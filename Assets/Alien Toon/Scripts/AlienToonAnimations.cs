using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class AlienToonAnimations:MonoBehaviour{
    float _counter;
    float _counterWait;
    bool _idle;
    
    public void Start() {
    GetComponent<Animation>().Play("idleCasual");
    }
    
    public void Update() {
    	_counter+=Time.deltaTime;
    	
    	if(Input.GetKeyUp("1")){
    		_counter = 0.0f;
    		GetComponent<Animation>().CrossFade("scout");
    		_counterWait = 4.0f;
    		_idle = false;
    	}else if(Input.GetKeyUp("2")){
    		 GetComponent<Animation>().CrossFade("hit",.2f);
    		 _counter = 0.0f;
    		 _counterWait = 1.0f;
    		 _idle = false;
    	}else if(Input.GetKeyUp("3")){
    		GetComponent<Animation>().CrossFade("angry");
    		_counter = 0.0f;
    		_counterWait = 5.0f;
    		_idle = false;
    	}else if(Input.GetKeyUp("4")){
    		GetComponent<Animation>().CrossFade("jawn");
    		_counter = 0.0f;
    		_counterWait = 3.0f;
    		_idle = false;
    	}else if(Input.GetKeyUp("5")){
    		GetComponent<Animation>().CrossFade("smile");
    		_counter = 0.0f;
    		_counterWait = 2.0f;
    		_idle = false;
    	}else if(Input.GetKeyUp("6")){
    		GetComponent<Animation>().CrossFade("death");
    		_counter = 0.0f;
    		_counterWait = 3.0f;
    		_idle = false;
    	}	
    	
    	if(_counter > _counterWait && !_idle){
    		GetComponent<Animation>().CrossFade("idleCasual");
    		_idle = true;
    	}
    }
}