
#include<Wire.h>

const int buttonPin = 9;
const int joyx = A0;
const int joyy = A1;
const int h1 = A2;
const int h2 = A3;



const int MPU1 = 0x68;
const int MPU2 = 0x69;
int16_t AcX, AcY, AcZ, Tmp, GyX, GyY, GyZ;
int16_t fAcX, fAcY, fAcZ, fTmp, fGyX, fGyY, fGyZ;
int16_t buttonState = 0;
int16_t sensorValue = 0;
// value returned is in interval [-32768, 32767] so for normalize multiply GyX and others for gyro_normalizer_factor
// float gyro_normalizer_factor = 1.0f / 32768.0f;

void setup() {
  Wire.begin();
  Wire.beginTransmission(MPU1);
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     // set to zero (wakes up the MPU-6050)
  Wire.endTransmission(true);
  Wire.begin();
  Wire.beginTransmission(MPU2);
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     // set to zero (wakes up the MPU-6050)
  Wire.endTransmission(true);
  pinMode(buttonPin, INPUT);


  Serial.begin(38400);
}


void loop() {
 

  int X = analogRead(joyx);
  int Y = analogRead(joyy);
  int16_t H1 =map( analogRead(h1),300,500,-264,245);//a2
  int16_t H2 =map( analogRead(h2),600,900,-700,104);//a3
  

  buttonState = digitalRead(buttonPin);


  Wire.beginTransmission(MPU1);
  Wire.write(0x3B);  // starting with register 0x3B (ACCEL_XOUT_H)
  Wire.endTransmission(false);
  Wire.requestFrom(MPU1, 14, true); // request a total of 14 registers
  AcX = Wire.read() << 8 | Wire.read(); // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)
  AcY = Wire.read() << 8 | Wire.read(); // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  AcZ = Wire.read() << 8 | Wire.read(); // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  Tmp = Wire.read() << 8 | Wire.read(); // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  GyX = Wire.read() << 8 | Wire.read(); // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  GyY = Wire.read() << 8 | Wire.read(); // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  GyZ = Wire.read() << 8 | Wire.read(); // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
  Wire.beginTransmission(MPU2);
  Wire.write(0x3B);  // starting with register 0x3B (ACCEL_XOUT_H)
  Wire.endTransmission(false);
  Wire.requestFrom(MPU2, 14, true); // request a total of 14 registers
  fAcX = Wire.read() << 8 | Wire.read(); // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)
  fAcY = Wire.read() << 8 | Wire.read(); // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  fAcZ = Wire.read() << 8 | Wire.read(); // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  fTmp = Wire.read() << 8 | Wire.read(); // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  fGyX = Wire.read() << 8 | Wire.read(); // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  fGyY = Wire.read() << 8 | Wire.read(); // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  fGyZ = Wire.read() << 8 | Wire.read(); // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)


Serial.print(AcX); Serial.print(";"); Serial.print(AcY); Serial.print(";"); Serial.print(AcZ); Serial.print(";");
Serial.print(GyX); Serial.print(";"); Serial.print(GyY); Serial.print(";"); Serial.print(GyZ); Serial.print(";");

 Serial.print(X); Serial.print(";"); Serial.print(Y); Serial.print(";"); 
 Serial.print(buttonState); Serial.print(";");
  Serial.print(H1); Serial.print(";"); Serial.print(H2);  Serial.print(";");
  
 Serial.print(fAcX); Serial.print(";"); Serial.print(fAcY); Serial.print(";"); Serial.print(fAcZ); Serial.print(";");
 Serial.print(fGyX); Serial.print(";"); Serial.print(fGyY); Serial.print(";"); Serial.print(fGyZ); 
  
  
  Serial.println("");
  Serial.flush();

  delay(25);
  }
