using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countLayerMark;


    private bool isWalking;
    // diem cuoi va cham
    private Vector3 lastInteractDir;
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    // ham vo hieu hoa 
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero )
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, moveDir, out RaycastHit raycastHit, interactDistance,countLayerMark))
        {
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter) )
            {
                clearCounter.Interact();
            }
        }

    }

    // ham co the di chuyen
    private void HandleMovement()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        // raycat ngan can ngupi choi ko vuot qua truong  ngai vat

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // khong the di chuyen len phia truoc
            // chi di chuyen duoc trong truc x
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);


            if (canMove)
            {
                // chi co the di chuyen trong truc x
                moveDir = moveDirX;
            }
            else
            {
                // khong the di chuyen theo chuc x
                // co the  theo truc z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // chi di theo truc z
                    moveDir = moveDirZ;
                }
                else
                {
                    // ko the di theo truc nao ca 
                }
            }
        }


        if (canMove)
        {
            transform.position += moveDir * moveDistance;

        }

        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        // forward huong truoc mat
        // Slerp chuyển đổi mượt mà các phép quay.
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

}
