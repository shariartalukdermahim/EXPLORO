 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO.Ports;
 
 
public class playermove : MonoBehaviour
{
  
    SerialPort stream;
    public CharacterController control ;
    public float speed = 6f ;
    public float gravity = -9.81f ;
 
    
     public GameObject handmove ;
     public GameObject headx ;
     public GameObject heady ;
     public GameObject headz ;

    float acc_normalizer_factor = 0.00025f;
    float gyro_normalizer_factor = 1.0f / 32768.0f;   // 32768 is max value captured during test on imu
 
    float curr_angle_x = 50;
    float curr_angle_y = 40;
    float curr_angle_z = 11;
    float curr_offset_x = 0;
    float curr_offset_y = 0;
    float curr_offset_z = 0;

 
    float fcurr_offset_x = 0;
    float fcurr_offset_y = 0;
    float fcurr_offset_z = 0;
 
     float fcurr_angle_x = 45;
    float fcurr_angle_y = 0;
    float fcurr_angle_z = 0;
 
 
    // Increase the speed/influence rotation
    public float factor = 7;
 
 
    public bool enableristRotation;
    public bool enableheadRotation;
    public bool enablehandmove;
 
    // SELECT YOUR COM PORT AND BAUDRATE
    string port = "COM4";
    int baudrate = 38400;
    int readTimeout = 25;
 Vector3 velocity ;


  float throwForce = 600;
 Vector3 objectPos;
 float distance;

 public bool canHold = true;
 public GameObject item;
 public GameObject tempParent;
 public bool isHolding = false;



    void Start()
    {   
        // open port. Be shure in unity edit > project settings > player is NET2.0 and not NET2.0Subset
        stream = new SerialPort("\\\\.\\" + port, baudrate);
 
        try
        {
            stream.ReadTimeout = readTimeout;
        }
        catch (System.IO.IOException ioe)
        {
            Debug.Log("IOException: " + ioe.Message);
        }
 
        stream.Open();
    }
 
    void Update()
    {


        
        string dataString = "null received";
 
        if (stream.IsOpen)
        {
            try
            {
                dataString = stream.ReadLine();
                Debug.Log("RCV_ : " + dataString);
            }
            catch (System.IO.IOException ioe)
            {
                Debug.Log("IOException: " + ioe.Message);
            }
 
        }
        else
            dataString = "NOT OPEN";
        Debug.Log("RCV_ : " + dataString);
 
        if (!dataString.Equals("NOT OPEN"))
        {
            // recived string is  like  "accx;accy;accz;gyrox;gyroy;gyroz"
            char splitChar = ';';
            string[] dataRaw = dataString.Split(splitChar);
 
            // normalized accelerometer values
            float ax = Int32.Parse(dataRaw[0]) * acc_normalizer_factor;
            float ay = Int32.Parse(dataRaw[1]) * acc_normalizer_factor;
            float az = Int32.Parse(dataRaw[2]) * acc_normalizer_factor;
 
            // normalized gyrocope values
            float gx = Int32.Parse(dataRaw[3]) * gyro_normalizer_factor;
            float gy = Int32.Parse(dataRaw[4]) * gyro_normalizer_factor;
            float gz = Int32.Parse(dataRaw[5]) * gyro_normalizer_factor;
             
            float fax = Int32.Parse(dataRaw[11]) * acc_normalizer_factor;
            float fay = Int32.Parse(dataRaw[12]) * acc_normalizer_factor;
            float faz = Int32.Parse(dataRaw[13]) * acc_normalizer_factor;
 
            // normalized gyrocope values
            float fgx = Int32.Parse(dataRaw[14]) * gyro_normalizer_factor;
            float fgy = Int32.Parse(dataRaw[15]) * gyro_normalizer_factor;
            float fgz = Int32.Parse(dataRaw[16]) * gyro_normalizer_factor;


            float b =Int32.Parse(dataRaw[8]);

                 float H1 =Int32.Parse(dataRaw[9]);
                  float H2 =Int32.Parse(dataRaw[10]);


            float x = Int32.Parse(dataRaw[6]) ;
            float y = Int32.Parse(dataRaw[7]) ;
            float XX = 0 ;
            float YY = 0 ;


            if(b==0)
            {

            if (distance <= 1f)
             {
              isHolding = true;
             item.GetComponent<Rigidbody> ().useGravity = false;
             item.GetComponent<Rigidbody> ().detectCollisions = true;
             }
            }
            else
              isHolding = false;




 if(x<450){
 XX = 1 ;
 }
  else if(x>500){
 XX = -1 ;
 } 
 else 
 XX = 0 ;
 if(y<450){
 YY = -1 ;
 }
 else if(y>550){
YY = 1 ;
 }  
 else
 YY = 0 ;
           
  
          Vector3 move = transform.right*-XX + transform.forward*YY ;

            control.Move(move * Time.deltaTime*speed) ;
 
            velocity.y += gravity * Time.deltaTime ;
            control.Move(velocity*Time.deltaTime) ;
            // prevent
            if (Mathf.Abs(ax) - 1 < 0) ax = 0;
            if (Mathf.Abs(ay) - 1 < 0) ay = 0;
            if (Mathf.Abs(az) - 1 < 0) az = 0;
 
 
            curr_offset_x += ax;
            curr_offset_y += ay;
            curr_offset_z += az; // The IMU module have value of z axis of 16600 caused by gravity
 
            if (Mathf.Abs(fax) - 1 < 0) fax = 0;
            if (Mathf.Abs(fay) - 1 < 0) fay = 0;
            if (Mathf.Abs(faz) - 1 < 0) faz = 0;
 
 
            fcurr_offset_x += fax;
            fcurr_offset_y += fay;
            fcurr_offset_z += faz;
            // prevent little noise effect
            if (Mathf.Abs(gx) < 0.025f) gx = 0f;
            if (Mathf.Abs(gy) < 0.025f) gy = 0f;
            if (Mathf.Abs(gz) < 0.025f) gz = 0f;
 
            curr_angle_x += gx;
            curr_angle_y += gy;
            curr_angle_z += gz;
             
            // prevent little noise effect
            if (Mathf.Abs(fgx) < 0.025f) fgx = 0f;
            if (Mathf.Abs(fgy) < 0.025f) fgy = 0f;
            if (Mathf.Abs(fgz) < 0.025f) fgz = 0f;
 
            fcurr_angle_x += fgx;
            fcurr_angle_y += fgy;
            fcurr_angle_z += fgz;


            distance = Vector3.Distance (item.transform.position, tempParent.transform.position);
  if (distance >= 1f) 
  {
   isHolding = false;
  }
  //Check if isholding
  if (isHolding == true) {
   item.GetComponent<Rigidbody> ().velocity = Vector3.zero;
   item.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
   item.transform.SetParent (tempParent.transform);

   
  }
  else 
  {
   objectPos = item.transform.position;
   item.transform.SetParent (null);
   item.GetComponent<Rigidbody> ().useGravity = true;
   item.transform.position = objectPos;
  }
 


            Transform hand = transform.Find("handrotation");
            
            
             
             if(enableristRotation)hand.localPosition =  new Vector3(H2/100, H1/100 , 4);
            if(enableheadRotation)headx.transform.localRotation = Quaternion.Euler(-fcurr_angle_x * factor, 0f, 0f);
            if(enableheadRotation)heady.transform.localRotation = Quaternion.Euler(0f , -fcurr_angle_y * factor,0f);
            if(enableheadRotation)headz.transform.localRotation = Quaternion.Euler(0f,0f * factor, fcurr_angle_z * factor);
 
 
            if(enablehandmove)handmove.transform.rotation =Quaternion.Euler(-curr_angle_x * factor, -curr_angle_y * factor, curr_angle_z * factor);
 
                
        }
    }
 
}