using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_Anim : MonoBehaviour {

    private Animator anim;
    private float vel;
    private bool crouch;
    private bool push;
    private bool pull;
    private bool carregando;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //vericifar se esta true ou false
        anim.SetFloat("Forward", vel);
        anim.SetBool("Crouch", crouch);
        anim.SetBool("Pull", pull);
        anim.SetBool("Push", push);
        anim.SetBool("Carregando", carregando);
    }

    // aumentar ou diminuir a velocidade
    public void AnimSpeed(float _vel)
    {
        vel = _vel;
    }

    //agachar se for verdadeiro
    public void Crouch(bool agachar)
    {
        crouch = agachar;
    }

    //empurrar se for verdadeiro
    public void Push(bool puxar)
    {
        push = puxar;
    }

    //puxar se for verdadeiro
    public void Pull(bool empurrar)
    {
        pull = empurrar;
    }

    //se for verdadeiro esta com o saco
    public void Carregando(bool carre)
    {
        carregando = carre;
    }

    //ativar o vault
    public void Vault()
    {
        anim.SetTrigger("Vault");
    }

    //ativar o soltar o saco
    public void Soltar()
    {
        anim.SetTrigger("soltando");
    }

    //ativar o pegando
    public void PickUp()
    {
        anim.SetTrigger("Pegando");
    }

    //ativar o hide
    public void Hide()
    {
        anim.SetTrigger("Hide");
    }

    //ativar o unhide
    public void UnHide()
    {
        anim.SetTrigger("Unhide");
    }

    public void PickMainObj()
    {
        anim.SetTrigger("Carregando");
    }

    //ativar o vasculhando
    public void Olhar()
    {
        anim.SetTrigger("See");
    }

    //ativar o jogar
    public void Throw()
    {
        int x = (int)Random.Range(1, 3);
        if(x ==1)
        {
            anim.SetTrigger("Throw1");
        }
       
        if (x == 2)
        {
            anim.SetTrigger("Throw0");
        }
        
    }
}
