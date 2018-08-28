using UnityEngine;
using System.Collections.Generic;
using System;





namespace Ardunity
{
    

        [AddComponentMenu("ARDUnity/Reactor/Transform/RotationReactor2S")]
    [HelpURL("https://sites.google.com/site/ardunitydoc/references/reactor/rotationreactor")]
	public class RotationReactor2S : ArdunityReactor
    {

        
         
        
        public Animator animator;
        public bool smoothFollow = true;

        float time = 0.0f;
        float time2 = 0.0f;
		private Quaternion _initRot;
		private IWireInput<Quaternion> _rotation;
        private Quaternion _curRotation;
		private Quaternion _fromRotation;
		private Quaternion _toRotation;
		private float _time;
        private Quaternion a;
        private bool k = true;
        private int count = 0;
        private int count1 = 0;
        private int decision = 0;
        public bool rightback = false;
        public bool leftback = false;
        public bool snow0,snow1,snow2 = false;

        protected override void Awake()
		{
            base.Awake();
			
			_initRot = transform.localRotation;
            _curRotation = Quaternion.identity;
            _toRotation =  Quaternion.identity;
                        
            _time = 1f;
            
        }
       

        
        // Use this for initialization
        void Start ()
		{
            
            
            animator = GetComponent<Animator>();
           
            

        }

        // Update is called once per frame
        void Update()
        {


            time += Time.deltaTime;
            

            if (_time < 1f && smoothFollow == true)
            {
                _time += Time.deltaTime;
                _curRotation = Quaternion.Lerp(_fromRotation, _toRotation, _time);
            }
            else
                _curRotation = _toRotation;
            //  print(_curRotation.x);
           
            Debug.Log("2번"+"x:"+_curRotation.x+" / "+"y:"+_curRotation.y+" / "+"z:"+_curRotation.z+" / "+"w:"+_curRotation.w);

            RotationReactorS _curRotation2 = GameObject.Find("snow").GetComponent<RotationReactorS>(); // 1번
            
            RotationReactor4 _curRotation4 = GameObject.Find("snow").GetComponent<RotationReactor4>(); // 4번

            //print(_curRotation2._curRotation);
            //print(_curRotation3._curRotation);
            if (_curRotation.x != 0)
            {
                count++;
            }
            if (count == 800)
            {
                a = _curRotation;
                //print(a);
                //transform.localRotation = _initRot * _curRotation;
                // k = false;
                

            }
            Debug.Log("2번초기값" + "x:" + a.x + " / " + "y:" + a.y + " / " + "z:" + a.z + " / " + "w:" + a.w);
            //print(a);



            
                   

           if (time >= 3.5f) 
               {
                    if (decision == 0) // 팔운동1 
                    {
                        if (Math.Abs(_curRotation2._curRotation.y) < 0.8f && Math.Abs(_curRotation2._curRotation.y) > 0.4f      // 1번 y: 0.4 < 현재값 < 0.8
                                && Math.Abs(_curRotation4._curRotation.y) < 0.8f && Math.Abs(_curRotation4._curRotation.y) > 0.4f) // 4번 y: 0.4 < 현재값 < 0.8
                        {

                            //animator.Play("Makehuman_gameskel|gun_game_1_bullet", -1, 0);  // 팔운동1 애니메이션 실행
                            count1++;
                            decision++;  // 팔운동 1 애니메이션이 실행되면 팔운동2 애니메이션이 실행될수있도록 decision = 1로 바꿔준다.
                            snow0 = true;
                            snow2 = false;
                       
                        }
                    }
               }

           else { }

            print("1번" + snow0);

           print("카운트값1: " + decision);


               if (time >= 3.5f) 
               {
               if (decision == 1) // 팔운동2
               {

                    if (Math.Abs(_curRotation2._curRotation.x) < 0.8f && Math.Abs(_curRotation2._curRotation.x) > 0.4f && Math.Abs(_curRotation2._curRotation.y) < 0.3f     // 1번 x: 0.4 < 현재값 < 0.8  y : 현재값 <0.3f
                         && Math.Abs(_curRotation4._curRotation.x) < 0.8f && Math.Abs(_curRotation4._curRotation.x) > 0.4f) // 4번 x: 0.4 < 현재값 < 0.8
                      
                   {   // and 3번 : 현재값 < 0.15f  and 4번 : 현재값 > 0.3f 

                        //animator.Play("Makehuman_gameskel|gun_game_2_sholder", -1, 0); // 팔운동2 애니메이션 실행
                        snow1 = true;
                        snow0 = false;
                       count1++;
                       decision++; // 팔운동2 애니메이션이 실행되면 팔운동3 애니메이션이 실행될수있도록 decision = 2로 바꿔준다.
                   }
               }
               }

           else { }

             print("카운트값2: " + decision);

            print("2번" + snow1);

            if (time >= 3.5f)
            {
                if (decision == 2)  // 팔운동3
                {
                    if (Math.Abs(_curRotation.z) < 0.4f ) // 2번 z: 현재값 < 0.4f
                        

                    {

                        //animator.Play("Makehuman_gameskel|gun_game_4_win", -1, 0); // 팔운동3 애니메이션 실행
                        snow2 = true;
                        snow1 = false;
                        count1++;
                        decision = 0; // 팔운동3 애니메이션이 실행되면 팔운동1 애니메이션이 실행될수있도록 decision = 0 으로 바꿔준다.

                    }
                }
            }

            else { }
            print("3번"+ snow2);

            
            // print("카운트값3: " + decision);

           

          



           






            if (count1 > 0)   //다리운동할때는 주석처리 / 허리와 팔은 필요함 
            {
                time = 0.0f;
                
                count1 = 0;
            }
            print(time);
            

        }








        private void OnRotationChanged(Quaternion q)
        {
            _fromRotation = _toRotation;
			_toRotation =  q;
            
            _time = 0f;
        }
		
		protected override void AddNode(List<Node> nodes)
        {
			base.AddNode(nodes);
			
			nodes.Add(new Node("rotation", "Rotation", typeof(IWireInput<Quaternion>), NodeType.WireFrom, "Input<Quaternion>"));
        }
        
        protected override void UpdateNode(Node node)
        {
            if(node.name.Equals("rotation"))
            {
                node.updated = true;
                if(node.objectTarget == null && _rotation == null)
                    return;
                
                if(node.objectTarget != null)
                {
                    if(node.objectTarget.Equals(_rotation))
                        return;
                }
                
                if(_rotation != null)
                    _rotation.OnWireInputChanged -= OnRotationChanged;
                
                _rotation = node.objectTarget as IWireInput<Quaternion>;
                if(_rotation != null)
                    _rotation.OnWireInputChanged += OnRotationChanged;
                else
                    node.objectTarget = null;
                
                return;
            }
            
            base.UpdateNode(node);
        }


	}
}
