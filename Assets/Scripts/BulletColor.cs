using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
public class BulletColor : MonoBehaviour {

	private SpriteRenderer rend; 
	private Rigidbody2D rb; 

	private float damage = 1; 
	private Tween tween;
    private Animator anim; 
	private bool isCharged = false; 
	private ParticleSystem particlesCharge; 
	private AutoDestroy autoDestroy; 

	// Use this for initialization
	void Awake () {
		this.rb = this.GetComponent<Rigidbody2D> (); 
		tween = null; 
		particlesCharge = this.GetComponentInChildren<ParticleSystem> (); 
		particlesCharge.gameObject.SetActive (false);
        anim = this.GetComponent<Animator>();
		autoDestroy = this.GetComponent<AutoDestroy> ();
		autoDestroy.enabled = false; 
	}



	public void QueueDestroy(){
		if (tween == null) {
			tween = DOVirtual.DelayedCall (0.1f, () => {
				Destroy (this.gameObject); 
			}
			);
		}
	}

	public void UpdateDamageBySecond(float aument ){
		damage += aument; 
	}

	public void DropBall(float speed){
		
		rb.velocity = Vector2.up * speed;
		autoDestroy.enabled = true; 

        anim.SetTrigger("suelto");
	}

	public void ChangeColor(Color color){

		if (rend == null) {
			rend = this.GetComponent<SpriteRenderer> (); 
		}
		rend.color = color; 
		particlesCharge.startColor = rend.color; 

	}


	public float Damage {
		get {
			return damage;
		}
	}

	public SpriteRenderer Rend {
		get {
			return rend;
		}
	}

	public bool IsCharged {
		set{
			isCharged = value; 
			particlesCharge.gameObject.SetActive (isCharged); 

		}
		get{
			return isCharged; 
		}
	}
}
