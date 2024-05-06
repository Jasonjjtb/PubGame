using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //general vars
    public float acceleration = 5f;
    public float deceleration = 8f;
    public float turnDecceleration = 10f;
    public float sprintTurnDecceleration_factor = 0.1f;
    public float maxSpeed = 6f;
    public float speed = 2f;
    public float sprintSpeed = 4f;  
    public float stamina = 50f, max_stamina = 50f;
    // Sprinting vars
    private bool isSprinting;
    public float sprintCost = 10f;
    public float staminaRegenRate = 20f;
    public float sprintCooldown = 5f; // Lockout period in seconds  
    // Dashing vars
    private bool canDash = true;
    private bool isDashing;
    public float dashingDistance = 24f; // Distance to dash
    private float dashingTime = 0.2f;
    public float dashCooldown = 3f; // Cooldown for dash in seconds
    // Misc vars
    private Rigidbody2D characterBody;
    private Vector2 inputMovement;
    public Animator animator;
    // Current vars
    private float currentCooldownSprint;
    private float currentCooldownDash;
    private Vector2 currentAcceleration;

    [SerializeField] private TrailRenderer tr;

    void Start()
    {
        characterBody = GetComponent<Rigidbody2D>();
        currentCooldownSprint = 0f;
        dashCooldown = 0f;
        canDash = true;
    }

    void Update()
    {
        if(isDashing){
            return;
        }
        
        inputMovement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Check for sprint input (shift)
        isSprinting = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && stamina > 0 && currentCooldownSprint <= 0f;

        // Adjust speed based on sprinting using ? operand
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        // Calculate velocity
        CalculateVelocity(ref currentSpeed);

        // Handle cooldowns
        if (currentCooldownSprint > 0f)
        {
            currentCooldownSprint -= Time.deltaTime;
            currentCooldownSprint = Mathf.Max(currentCooldownSprint, 0f);
        }

        if (canDash == false)
        {
            dashCooldown -= Time.deltaTime;
            dashCooldown = Mathf.Max(dashCooldown, 0f);
        }

        // Reduce stamina when sprinting
        if (isSprinting)
        {
            stamina -= sprintCost * Time.deltaTime;
            stamina = Mathf.Max(stamina, 0f);
            if (stamina <= 0f)
            {
                currentCooldownSprint = sprintCooldown;
            }
        }
        else
        {
            // Regenerate stamina when not sprinting
            if (stamina < max_stamina)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Min(stamina, max_stamina);
            }
        }

        // Handle dash cooldown
        if (dashCooldown == 0f)
        {
            canDash = true;
            dashCooldown = 3f;
        }

        // Perform dash
        if (Input.GetKey(KeyCode.Space) && canDash)
        {
        Debug.Log("Dash initiated");
        StartCoroutine(Dash());
        }

        // Debug.Log("Current Speed: " + currentSpeed);
    }

    private void FixedUpdate()
    {
        if(isDashing){
            return;
        }

        // Check for sprint input (shift)
        isSprinting = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && stamina > 0 && currentCooldownSprint <= 0f;

        // Adjust speed based on sprinting using ? operand
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        // Calculate the desired velocity based on input and speed
        Vector2 targetVelocity = inputMovement.normalized * currentSpeed;
        
        //Debug.Log("Max Speed: " + maxSpeed);
        //Debug.Log("currentAcceleration: " + currentAcceleration.magnitude);
        // Apply force to move the character
        float conditionalmaxSpeed = isSprinting ? sprintSpeed : maxSpeed;
        float conditonal_turnDecceleration = turnDecceleration;
        if (isSprinting) {
            conditonal_turnDecceleration *= sprintTurnDecceleration_factor;
        }
        if (characterBody.velocity.magnitude < conditionalmaxSpeed){
            characterBody.AddForce(inputMovement * currentAcceleration.magnitude, ForceMode2D.Force);
        } else {
            Vector2 scaled_input = conditionalmaxSpeed * inputMovement;
            characterBody.velocity = Vector2.Lerp(characterBody.velocity, scaled_input, Time.deltaTime * conditonal_turnDecceleration);
        }

        if (inputMovement.magnitude == 0f){
            characterBody.velocity = Vector2.Lerp(characterBody.velocity, Vector2.zero, Time.deltaTime * deceleration);
        }
        else{
        // If there is no input, apply additional deceleration
            if (inputMovement.x == 0f)
            {
                Vector2 frictionVector = new Vector2(-1*characterBody.velocity.x, 0);
                characterBody.AddForce(frictionVector * turnDecceleration, ForceMode2D.Force);
            }

            if (inputMovement.y == 0f)
            {
                Vector2 frictionVector = new Vector2(0, -1*characterBody.velocity.y);
                characterBody.AddForce(frictionVector * turnDecceleration, ForceMode2D.Force);
            }
        }
    }

    private void CalculateVelocity(ref float speed)
    {
        // Smoothly adjust velocity based on input and current velocity
        currentAcceleration = Vector2.Lerp(currentAcceleration, inputMovement * speed, Time.deltaTime * acceleration);

        // Limit speed to the maximum allowed speed
        currentAcceleration = Vector2.ClampMagnitude(currentAcceleration, maxSpeed);

        // Set animator parameters
        animator.SetFloat("Horizontal", inputMovement.x);
        animator.SetFloat("Vertical", inputMovement.y);
        animator.SetFloat("Speed", inputMovement.sqrMagnitude * speed / this.speed);
    }

    private IEnumerator Dash(){
        // Check if there is any movement input
        if (inputMovement.magnitude > 0.0f)
        {
            canDash = false;
            isDashing = true;

            // Calculate the dash direction based on the input movement
            Vector2 dashDirection = inputMovement.normalized;

            // Apply force for the dash
            characterBody.AddForce(dashDirection * dashingDistance, ForceMode2D.Impulse);

            // Enable the trail renderer
            tr.emitting = true;

            // Wait for the dash time
            yield return new WaitForSeconds(dashingTime);

            // Disable the trail renderer
            tr.emitting = false;

            isDashing = false;
            canDash = false;
        }
        // Reset canDash only if there is movement input
        else
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }
}
