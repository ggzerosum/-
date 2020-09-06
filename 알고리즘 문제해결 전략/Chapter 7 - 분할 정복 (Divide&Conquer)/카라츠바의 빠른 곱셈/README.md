### 후기
카라츠바 알고리즘은 시간 복잡도(빅오)가 일반적인 곱셈보다 빠르지만,
연산이 훨씬 더 복잡하기 때문에 짧은 수의 곱셈은 오히려 느리는다는 것에 유의하자.

### 증명 및 빅오
![증명 및 빅오](https://github.com/ggzerosum/Algorithm-Solving/blob/master/%EC%95%8C%EA%B3%A0%EB%A6%AC%EC%A6%98%20%EB%AC%B8%EC%A0%9C%ED%95%B4%EA%B2%B0%20%EC%A0%84%EB%9E%B5/Chapter%207%20-%20%EB%B6%84%ED%95%A0%20%EC%A0%95%EB%B3%B5%20(Divide&Conquer)/%EC%B9%B4%EB%9D%BC%EC%B8%A0%EB%B0%94%EC%9D%98%20%EB%B9%A0%EB%A5%B8%20%EA%B3%B1%EC%85%88/%EC%A6%9D%EB%AA%85%EA%B3%BC%20BigO.jpg?raw=true)


## 교훈
### 카라츠바 알고리즘에서 배울 수 있는 코딩 노하우
##### 1) 원하는 조건을 만족시키기위해 재귀호출을 사용할 수 있다.
```C#
        public List<int> Multiply(List<int> a, List<int> b)
        {
            // a가 반드시 b보다 긴 경우만 다루기 위해서
            // a가 b보다 짧으면 예외처리 대신 순서를 바꾸어 재귀호출
            if (a.Count < b.Count)
                return Multiply(b, a);
                
            // 이하 코드 생략
        }
```
##### 2) 숫자를 배열에 담을 때 거꾸로 담으면 편하다.
1234 (천이백삼십사)라는 숫자를 배열에 담을 때, 10진수 자릿수를 기준으로
{4(10^0), 3(10^1), 2(10^2), 1(10^3)} 로 담으면 10진수 자릿수로 표현할 수 있어서 매우 편하다.

```C#
int[] _number = new int[4] {4, 3, 2, 1}
```

##### 3) 배열에서 숫자 반올림(Carry)을 편하기 하기 위해 0을 맨앞에 추가한뒤 계산을 진행하고, 0이 남으면 제거해주는 방식
