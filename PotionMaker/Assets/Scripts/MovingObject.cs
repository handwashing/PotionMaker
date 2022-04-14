
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public LayerMask layerMask; //충돌했을 때 어떤 레이어와 부딪혔는지 분별해
                                //통과가 불가능한 레이어인지 판단해주는 변수

    public float speed;

    private Vector3 vector;

    public int walkCount;
    private int currentWalkCount;

    //for Coroutine 반복 시행 방지를 위해 변수 선언
    private bool canMove = true;

    private Animator animator;

    //speed = 2.4, walkCount = 20 -> 2.4 * 20 = 48
    //한 번 방향키가 눌릴 때마다 48px씩 이동시키겠다~
    //매번 곱할수 는 없으니 while 반복문 사용
    //currentWalkCount += 1, 20 /1씩 증가하며 20이 될 경우 반복문에서 빠져나옴 


    void Start()
    {
        animator = GetComponent<Animator>();
        //boxCollider = GetComponent<BoxCollider2D>();
        //GetComponent함수를 이용해 컴퍼넌트를 불러온다...
    }

    IEnumerator MoveCoroutine() //함수가 동시 실행된다 - 다중처리의 개념
    {
        while(Input.GetAxisRaw("Vertical")!=0||Input.GetAxisRaw("Horizontal") != 0)
        {
        vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);
           
            if (vector.x != 0) //x가 0이 아니면 y는 0
                vector.y = 0;

            animator.SetFloat("DirX", vector.x);
        animator.SetFloat("DirY", vector.y);
        animator.SetBool("Walking", true);

        while (currentWalkCount < walkCount)
        {
            //while문 안에 이동을 담당하는 알고리즘을 넣고 한 회차 마다 current를 1씩 상승
            if (vector.x != 0)
            {
                transform.Translate(vector.x * speed, 0, 0);
                //transform.position = vector;
            }
            else if (vector.y != 0)
            {
                transform.Translate(0, vector.y * speed, 0);
            }

            currentWalkCount++;
            yield return new WaitForSeconds(0.01f);
        }
        currentWalkCount = 0; //반복문에서 빠져나오면 다시 0으로 초기화
                              //yield return new WaitForSeconds(1f); //waiting 1 second -> 시간 조절위해 반복문 안으로 이동

        }
        animator.SetBool("Walking", false);
        canMove = true; //방향키 처리가 가능하도록...
    }
    //    //실질적으로 이동이 이루어지지 않도록 명령어를 짜준다
    //    RaycastHit2D hit;
    //    //A->B 레이저/ B에 레이저 무사히 도달 => hit = Null;
    //    //그 반대로 도달x, 방해물 충돌 => hit = 방해물 (리턴 되는 것)

    //    //A,B지점에 해당하는 변수를 선언해주자...

    //    Vector2 start = transform.position; //A지점-캐릭터의 현재 위치 값
    //    Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);   //B지점-캐릭터가 이동하고자 하는 위치 값

    //    //캐릭터가 스스로의 값을 가지고 있음 -> 자기 스스로의 박스콜라이더 충돌을 막기 위해 잠시 꺼주기
    //    boxCollider.enabled = false;
    //    hit = Physics2D.Linecast(start, end, layerMask);
    //    boxCollider.enabled = true; //다시 원위치 시켜주기

    //    if (hit.transform != null) //hit에 반환되는 값이 있을 경우 이후 명령 실행 x
    //                               //break;                 //A->B레이저 발사했을 때 layerMask에 해당하는 벽이 있다면 이후 실행 x
    //                               //animator.SetBool("Walking", true);

        
    //}


    void Update()
    {//Input.GetAxisRaw("Horizontal")우 방향키가 눌리면 1리턴, 좌 방향키가 눌리면 -1리턴
     //Input.GetAxisRaw("Vertical")상 방향키가 눌리면 1 리턴, 하 방향키가 눌리면 -1 리턴

        if (canMove) //canMove 상태에만 시행되도록...
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false; //한 번 시행후 또 시행되지 않도록
                StartCoroutine(MoveCoroutine());
                //방향키가 눌렸을 때 Coroutine이 동시에 여러개가 시행
            }

        }

    }
}
