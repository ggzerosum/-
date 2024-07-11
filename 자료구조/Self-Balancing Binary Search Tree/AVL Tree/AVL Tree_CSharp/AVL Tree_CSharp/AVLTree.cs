namespace ggzerosum.Datastructure
{
    public class AVLTree_Sketch
    {
        public class Node
        {
            public enum UnBalanceState
            {
                LL,
                RR,
                LR,
                RL
            }
            
            public enum Direction
            {
                Invalid = -1,
                NotWeighted = -2,
                
                Left = 0,
                Right = 1
            }
            
            public int value;
            
            // 자기 자신을 높이 1로 치겠습니다. 서브트리가 없을 경우, 0값이 나오도록 유도하려는 의도입니다.
            public int Height = 1;
            
            public Node Parent;

            private Node _left;
            public Node Left
            {
                get => _left;
                internal set
                {
                    Node v = value;

                    if (_left != null)
                    {
                        if (_left.Parent == this)
                            _left.Parent = null;
                        
                        _left = null;
                    }

                    if (v != null)
                    {
                        v.Parent = this;
                    }
                    this._left = v;
                }
            }

            private Node _right;
            public Node Right
            {
                get => _right;
                internal set
                {
                    Node v = value;

                    if (_right != null)
                    {
                        if (_right.Parent == this)
                            _right.Parent = null;
                        
                        _right = null;
                    }

                    if (v != null)
                    {
                        v.Parent = this;
                    }
                    this._right = v;
                }
            }

            public bool HasParent() => Parent != null;
            public bool HasLeftChild() => Left != null;
            public bool HasRightChild() => Right != null;
            
            /// <summary>
            /// 항상 노드를 추가할 때마다 Balance를 조절해왔을 때 SubTree가 UnBalance하려면 항상 AVL 트리가 제시하는 4가지 경우의 수만 나옵니다.
            /// </summary>
            public bool IsSubtreeUnBalance()
            {
                int factor = BalanceFactor;
                return factor < -1 || factor > 1;
            }

            public int BalanceFactor => rightSubTreeHeight - leftSubTreeHeight;
            
            private int leftSubTreeHeight => HasLeftChild() ? Left.Height : 0;
            private int rightSubTreeHeight => HasRightChild() ? Right.Height : 0;

            public Node(int value)
            {
                this.value = value;
                this.Height = 1;
            }

            public void RecomputeHeight(bool recursive)
            {
                RecomputeHeight(this, recursive);
            }
            private void RecomputeHeight(Node node, bool recursive)
            {
                // 자기 자신만 있더라도 높이 1로 치겠다고 약속했으므로, 자식들의 최대 높이 + 1을 해야 자신의 높이가 됩니다.
                node.Height = 1 + Math.Max(node.leftSubTreeHeight, node.rightSubTreeHeight);
                
                if (recursive && node.HasParent())
                    RecomputeHeight(node.Parent, recursive);
            }
            
            /// <summary>
            /// 밸런스 조절은 항상 대상이 되는 노드를 최상위로 올리기 때문에 꼭 Node를 명시적으로 올릴 필요는 없지만, 보다 명확한 코딩을 위해 상위로 올라오는 노드를 명시적으로 반환하겠습니다.
            /// </summary>
            public void BalanceSubTree()
            {
                if (!IsSubtreeUnBalance())
                    return;
                
                UnBalanceState unBalanceState = GetBalanceState();
                if (unBalanceState == UnBalanceState.LL)
                {
                    // 현재 노드는 제일 위 노드이고, 자식으로 3개가 있을 거라고 가정합니다.
                    RotateRight(this.Left);
                }
                else if (unBalanceState == UnBalanceState.RR)
                {
                    // 현재 노드는 제일 위 노드이고, 자식으로 3개가 있을 거라고 가정합니다.
                    RotateLeft(this.Right);
                }
                else if (unBalanceState == UnBalanceState.LR)
                {
                    var terminalNode = this.Left.Right;
                    RotateLeft(terminalNode);
                    RotateRight(terminalNode);
                }
                else if (unBalanceState == UnBalanceState.RL)
                {
                    var terminalNode = this.Right.Left;
                    RotateRight(terminalNode);
                    RotateLeft(terminalNode);
                }
            }
            
            private UnBalanceState GetBalanceState()
            {
                Direction first = GetWeightedDirection(this);
                if (first < 0)
                    throw new ArgumentException("Error, Child Node Perfectly balanced");
                
                // Balance는 노드를 하나씩 넣거나 뺄 때마다 체크할 것이므로, 항상 UnBalance는 좌/우 둘 중 한 방향에만 발생한다는 전제를 깔고 있습니다. 
                Direction second;
                second = GetWeightedDirection(this.Left);
                if (second < 0)
                    second = GetWeightedDirection(this.Right);
                
                if (second < 0)
                    throw new ArgumentException("Error, Child Node Perfectly balanced");
                
                
                if (first == Direction.Left && second == Direction.Left)
                    return UnBalanceState.LL;
                else if (first == Direction.Right && second == Direction.Right)
                    return UnBalanceState.RR;
                else if (first == Direction.Left && second == Direction.Right)
                    return UnBalanceState.LR;
                else if (first == Direction.Right && second == Direction.Left)
                    return UnBalanceState.RL;
                
                
                throw new ArgumentException($"cannot find proper {nameof(UnBalanceState)}");
            }
            
            private Direction GetWeightedDirection(Node node)
            {
                if (node == null)
                    return Direction.Invalid;
                
                if (node.BalanceFactor == 0)
                    return Direction.NotWeighted;
                
                return node.BalanceFactor > 0 ? Direction.Right : Direction.Left;
            }
            
            private Node RotateLeft(Node node)
            {
                return Rotate(node, true);
            }
            private Node RotateRight(Node node)
            {
                return Rotate(node, false);
            }
            /// <summary>
            /// 노드를 회전시키고, 3개의 노드 중 Root가 되는 노드를 반환합니다.
            /// </summary>
            private Node Rotate(Node node, bool leftOrRight)
            {
                var parent = node.Parent;
                var ancestor = parent.Parent;
                
                if (leftOrRight)
                {
                    // 현재 노드의 부모 노드를 떼어 현재 노드의 왼쪽에 붙입니다.
                    // LeftRotation은 부모 노드의 오른편에 있는 타겟 노드를 대상으로 합니다.
                    // 부모 노드를 떼어 타겟 노드에 붙일 것이므로, 부모 노드의 오른편 노드 연결을 해제해야합니다.
                    parent.Right = null;
                    
                    // 타겟 노드의 왼편에 노드가 있었을 경우, 부모 노드의 우편에 붙입니다.
                    // 부모 노드의 우편은 타겟 노드를 떼어냈기 때문에 항상 비어있습니다.
                    if (node.HasLeftChild())
                    {
                        var left = node.Left;
                        node.Left = null;
                        parent.Right = left;
                    }
                    node.Left = parent;
                    
                    node.Parent = ancestor;
                    // Ancestor가 null일 수도 있음 (Tree의 Root일 수도 있음)
                    if (ancestor != null)
                        ancestor.Right = node;
                }
                else
                {
                    // 현재 노드의 부모 노드를 떼어 현재 노드의 왼쪽에 붙입니다.
                    // LeftRotation은 부모 노드의 오른편에 있는 타겟 노드를 대상으로 합니다.
                    // 부모 노드를 떼어 타겟 노드에 붙일 것이므로, 부모 노드의 오른편 노드 연결을 해제해야합니다.
                    parent.Left = null;

                    if (node.HasRightChild())
                    {
                        var right = node.Right;
                        node.Right = null;
                        parent.Left = right;
                    }
                    
                    node.Right = parent;
                    node.Parent = ancestor;
                    // Ancestor가 null일 수도 있음 (Tree의 Root일 수도 있음)
                    if (ancestor != null)
                        ancestor.Left = node;
                }
                
                // 다 붙이고나면, Height를 다시 계산해서 기록합니다.
                // 부모 노드가 타겟 노드의 자식 노드가 되었으므로, 부모 노드였던 노드부터 Height를 계산해야 올바릅니다.
                // Recursive로 자식에서 부모로 올라가며 Height를 전부 주입해주면 간편하게 해결됩니다.
                parent.RecomputeHeight(true);
                
                // AVL Tree의 알고리즘은 회전을 시킬 대상 노드인 3개 노드의 Root노드로 만듭니다.
                return node;
            }
        }
        
        public Node _root;
        
        public void Add(int item)
        {
            Node leafNode = new Node(item);
            if (_root == null)
            {
                _root = leafNode;
                return;
            }
            
            AddInternal(_root, leafNode);
            
            // Balance를 조절하면 Root가 바뀔 수도 있기 때문에, Leaf노드에서 Root까지 올라가며 계속 밸런스를 조절합니다.
            // 밸런스 조절은 항상 대상이 되는 노드를 최상위로 올리기 때문에 마지막 밸런싱 대상인 노드가 'Root'가 될 수 있습니다. 
            Node root = RebalanceBottomToUp(leafNode);
            _root = root;
        }
        private void AddInternal(Node parent, Node leaf)
        {
            bool insertOnLeft = leaf.value <= parent.value;
            if (insertOnLeft)
            {
                if (parent.HasLeftChild())
                {
                    AddInternal(parent.Left, leaf);   
                }
                else
                {
                    parent.Left = leaf;
                    parent.RecomputeHeight(true);
                }
            }
            else
            {
                if (parent.HasRightChild())
                {
                    AddInternal(parent.Right, leaf);   
                }
                else
                {
                    parent.Right = leaf;
                    parent.RecomputeHeight(true);
                }
            }
        }
        
        /// <summary>
        /// Balance를 조절하면 Root가 바뀔 수도 있기 때문에, Leaf노드에서 Root까지 올라가며 계속 밸런스를 조절합니다.
        /// 밸런스 조절은 항상 대상이 되는 노드를 최상위로 올리기 때문에 마지막 밸런싱 대상인 노드가 'Root'가 될 수 있습니다. 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node RebalanceBottomToUp(Node node)
        {
            node.BalanceSubTree();
            // 밸런스 조절은 항상 대상이 되는 노드를 더 위로 올리기 때문에, Leaf에서 Root까지 올라가며 재귀적으로 리밸런싱할 때 밑의 논리가 성립합니다.
            if (node.HasParent())
            {
                return RebalanceBottomToUp(node.Parent);
            }
            
            return node;
        }
        
        public string PrintTree()
        {
            string treeDescription = "";
            Queue<Node> breathFirst = new Queue<Node>();
            breathFirst.Enqueue(_root);

            while (breathFirst.Any())
            {
                Node node = breathFirst.Dequeue();
                
                string info = GetNodeInfo(node);
                treeDescription += $"{info}\n";
                
                if (node.HasLeftChild())
                    breathFirst.Enqueue(node.Left);
                if (node.HasRightChild())
                    breathFirst.Enqueue(node.Right);
            }

            return treeDescription;
        }
        private string GetNodeInfo(Node node)
        {
            string header = $"[{node.value} 노드 정보] ";

            string height = $"<높이>:{node.Height}";
            
            string leftInfo = "<Left>:";
            leftInfo += node.HasLeftChild() ? $"{node.Left.value}노드" : "없음";
            
            string rightInfo = "<Right>:";
            rightInfo += node.HasRightChild() ? $"{node.Right.value}노드" : "없음";
            
            return header + height + "/" + leftInfo + "/" + rightInfo;
        }
    }
}