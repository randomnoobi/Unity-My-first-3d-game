using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons; // Reference to weapons
    private int currentWeaponIndex = 0;
    public Animator AxeAnimator;
    public Animator BowAnimator;
    private bool cancelbow = false;
    private bool isDrawing = false;

    public float requiredDrawTime = 1.0f;  // seconds to hold before allowing fire
    private float drawTimer = 0f;

    private void Start()
    {
        EquipWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);

        if (Input.GetMouseButtonDown(0))
        {
            AxeAnimator.SetTrigger("AttackTrigger");
        }

        if (Input.GetMouseButtonDown(1) && !isDrawing)
        {
            Debug.Log("Started Drawing Bow");
            isDrawing = true;
            cancelbow = false;
            drawTimer = 0f;
            BowAnimator.SetBool("mousehold", true);
            BowAnimator.SetBool("isFiring", false);
        }

        if (isDrawing)
        {
            drawTimer += Time.deltaTime;

            if (Input.GetKeyDown("x") || Input.GetMouseButtonDown(0))
            {
                Debug.Log("Cancelled drawing bow");
                cancelbow = true;
                isDrawing = false;
                drawTimer = 0f;
                BowAnimator.SetBool("mousehold", false);
                BowAnimator.SetBool("isFiring", false);
            }
        }

        if (isDrawing && Input.GetMouseButtonUp(1))
        {
            if (!cancelbow && drawTimer >= requiredDrawTime)
            {
                Debug.Log("Firing arrow");
                BowAnimator.SetTrigger("fire");
                BowAnimator.SetBool("isFiring", true);
            }
            else
            {
                Debug.Log("Draw released too early or cancelled");
                BowAnimator.SetBool("isFiring", false);
            }
            BowAnimator.SetBool("mousehold", false);
            isDrawing = false;
            drawTimer = 0f;
            cancelbow = false;
        }

        // Sanity check:
        if (!isDrawing && BowAnimator.GetBool("mousehold"))
        {
            Debug.LogWarning("mousehold was true while not drawing, resetting");
            BowAnimator.SetBool("mousehold", false);
        }
        if (isDrawing)
        {
            drawTimer += Time.deltaTime;

            if (Input.GetKeyDown("x"))
            {
                Debug.Log("X key pressed during drawing");
                // cancel logic here...
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Left mouse button pressed during drawing");
                // cancel logic here...
            }

            if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Right mouse button released");
                // firing logic here...
            }
        }

    }



    void EquipWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }
        currentWeaponIndex = index;
    }
    private IEnumerator ResetCancelBowNextFrame()
    {
        yield return null; // wait one frame
        cancelbow = false;
    }
}