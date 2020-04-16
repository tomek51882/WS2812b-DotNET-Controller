#include <Adafruit_NeoPixel.h>
#include <Wire.h>
//nano
//A5 SCL zielony 
//A4 SDA zółty
//micro
//3 SCL
//2 SDA
Adafruit_NeoPixel statusDiode = Adafruit_NeoPixel(1, 4, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel ledStrip = Adafruit_NeoPixel(150, 5, NEO_GRB + NEO_KHZ800);

byte SerialByteIn;
String SerialReadBuffer;
bool writingMode=false; //Arduino oczekuje na otrzymanie danych to zapisania
bool writingPattern1 = false;
bool writingPattern2 = false;
bool writingPattern3 = false;
bool writingPattern4 = false;
bool writingPattern5 = false;
bool writingPattern6 = false;
bool writingPattern7 = false;
bool writingPattern8 = false;
bool writingPattern9 = false;
bool writingPattern10 = false;
byte packetsComplete;
byte ledBuffer[450];
                
void setup() {
  pinMode(12,OUTPUT);
  Serial.begin(9600);
  Wire.begin();
  statusDiode.begin();
  statusDiode.show();
  ledStrip.begin();
  ledStrip.show();
  SerialReadBuffer= "";
  packetsComplete=0;
  //BlinkRGB(0,0,255);
  digitalWrite(12,HIGH);
  
  Serial.println("EEPROM controller: v1.0.2 Animation controller: None\nConnected leds: 150\nReady to work!");
}

void loop() {
  //WGRYWANIE WZORÓW
  if(writingMode)
  {
    //zmienić to w uniwersalną metodę
    if(writingPattern1 ==true)
    {
      savingPattern(0);
      writingPattern1 = false;
    }
    if(writingPattern2 ==true)
    {
      savingPattern(1);
      writingPattern2 = false;
    }
    if(writingPattern3 ==true)
    {
      savingPattern(2);
      writingPattern3 = false;
    }
    if(writingPattern4 ==true)
    {
      savingPattern(3);
      writingPattern4 = false;
    }
    if(writingPattern5 ==true)
    {
      savingPattern(4);
      writingPattern5 = false;
    }
    if(writingPattern6 ==true)
    {
      savingPattern(5);
      writingPattern6 = false;
    }
    if(writingPattern7 ==true)
    {
      savingPattern(6);
      writingPattern7 = false;
    }
    if(writingPattern8 ==true)
    {
      savingPattern(7);
      writingPattern8 = false;
    }
    if(writingPattern9 ==true)
    {
      savingPattern(8);
      writingPattern9 = false;
    }
    if(writingPattern10 ==true)
    {
      savingPattern(9);
      writingPattern10 = false;
    }
  }
  else
  {
    while (Serial.available()) 
    {
    SerialByteIn = Serial.read();
    SerialReadBuffer+=(char)SerialByteIn;
    delay(5);
    }
  }
  
  if(SerialReadBuffer =="CONN")
  {
    Serial.println("OK");
    BlinkRGB(0,255,0);
  }
  if(SerialReadBuffer =="SET_PATTERN_1")
  {
    writingMode=true;
    writingPattern1 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="SET_PATTERN_2")
  {
    writingMode=true;
    writingPattern2 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="SET_PATTERN_3")
  {
    writingMode=true;
    writingPattern3 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="SET_PATTERN_4")
  {
    writingMode=true;
    writingPattern4 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="SET_PATTERN_5")
  {
    writingMode=true;
    writingPattern5 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="SET_PATTERN_6")
  {
    writingMode=true;
    writingPattern6 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="SET_PATTERN_7")
  {
    writingMode=true;
    writingPattern7 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="SET_PATTERN_8")
  {
    writingMode=true;
    writingPattern8 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="SET_PATTERN_9")
  {
    writingMode=true;
    writingPattern9 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="SET_PATTERN_10")
  {
    writingMode=true;
    writingPattern10 = true;
    packetsComplete=0;
  }
  if(SerialReadBuffer =="GET_PATTERN_1")
  {
    readPattern(0x50, 0);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="GET_PATTERN_2")
  {
    readPattern(0x50, 1);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="GET_PATTERN_3")
  {
    readPattern(0x50, 2);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="GET_PATTERN_4")
  {
    readPattern(0x50, 3);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="GET_PATTERN_5")
  {
    readPattern(0x50, 4);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="GET_PATTERN_6")
  {
    readPattern(0x50, 5);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="GET_PATTERN_7")
  {
    readPattern(0x50, 6);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="GET_PATTERN_8")
  {
    readPattern(0x50, 7);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="GET_PATTERN_9")
  {
    readPattern(0x50, 8);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="GET_PATTERN_10")
  {
    readPattern(0x50, 9);
    delay(5);
    packetsComplete=0;
    Serial.write(ledBuffer, sizeof(ledBuffer));
  }
  if(SerialReadBuffer =="ON_PATTERN_1")
  {
    readPattern(0x50, 0);
    delay(100);
    showLeds();
  }
  if(SerialReadBuffer =="ON_PATTERN_2")
  {
    readPattern(0x50, 1);
    delay(5);
    showLeds();
  }
  if(SerialReadBuffer =="ON_PATTERN_3")
  {
    readPattern(0x50, 2);
    delay(5);
    showLeds();
  }
  if(SerialReadBuffer =="ON_PATTERN_4")
  {
    readPattern(0x50, 3);
    delay(5);
    showLeds();
  }
  if(SerialReadBuffer =="ON_PATTERN_5")
  {
    readPattern(0x50, 4);
    delay(5);
    showLeds();
  }
  if(SerialReadBuffer =="ON_PATTERN_6")
  {
    readPattern(0x50, 5);
    delay(5);
    showLeds();
  }
  if(SerialReadBuffer =="ON_PATTERN_7")
  {
    readPattern(0x50, 6);
    delay(5);
    showLeds();
  }
  if(SerialReadBuffer =="ON_PATTERN_8")
  {
    readPattern(0x50, 7);
    delay(5);
    showLeds();
  }
  if(SerialReadBuffer =="ON_PATTERN_9")
  {
    readPattern(0x50, 8);
    delay(5);
    showLeds();
  }
  if(SerialReadBuffer =="ON_PATTERN_10")
  {
    readPattern(0x50, 9);
    delay(5);
    showLeds();
  }
  if(SerialReadBuffer =="OFF_PATTERN")
  {
    for(int i=0;i<150;i++)
    {
      ledStrip.setPixelColor(i,0, 0, 0);
    }
    ledStrip.show();
  }
  SerialClearBuffer();
}
void showLeds()
{
  for(int i = 0; i<150;i++)
  {
    ledStrip.setPixelColor(i,ledBuffer[i*3], ledBuffer[(i*3)+1], ledBuffer[(i*3)+2]);
  }
  ledStrip.setPixelColor(149,0,0,0);
  ledStrip.show();
}
void savingPattern(int pattern)
{
  int counter=0;
  //Serial.println("Waiting for data...");
  while(!Serial.available()){BlinkRGB(255,0,0);}
  //Serial.println("Data received. Writting Pattern One...");
  while(packetsComplete<15)
  {
    while(Serial.available()<30){BlinkRGB(255,0,0);}
    delay(50);
    while(Serial.available())
    {
      ledBuffer[counter] = Serial.read();
      counter++;
    }
    packetsComplete++;
    delay(100);
    Serial.flush();
    Serial.print("ACK");
  }
  delay(50);
  Serial.print("FIN");
  writePattern(0x50,pattern, ledBuffer);
  delay(50);
  writingMode=false;
  BlinkRGB(0,255,0);
  Serial.println(F("Done"));
}
void SerialClearBuffer()
{
  SerialReadBuffer="";
}
void BlinkRGB(int r, int g, int b)
{
  statusDiode.setPixelColor(0, r, g, b);
  statusDiode.show();
  delay(50);
  statusDiode.setPixelColor(0, 0, 0, 0);
  statusDiode.show();
  delay(50);
  statusDiode.setPixelColor(0, r, g, b);
  statusDiode.show();
  delay(50);
  statusDiode.setPixelColor(0, 0, 0, 0);
  statusDiode.show();
  delay(50);
}
byte readAddress(byte device_Address, int address, int length)
{
  byte rData = 0xFF;
  Wire.beginTransmission(device_Address);
  Wire.write((int)(address >> 8));   // MSB
  Wire.write((int)(address & 0xFF)); // LSB
  Wire.endTransmission();  
  Wire.requestFrom(device_Address, length);  
  while(Wire.available())
  {
    rData = Wire.read();
  }
  return rData;
}
void readPattern(byte device_Address, int pattern)
{
  int address = 512*pattern;
  int counter=0;
  for(int i=0;i<15;i++)
  {
    Wire.beginTransmission(device_Address);
    Wire.write((int)(address >> 8));   // MSB
    Wire.write((int)(address & 0xFF)); // LSB
    Wire.endTransmission();  
    Wire.requestFrom(device_Address, 30);  
    while(Wire.available())
    {
      ledBuffer[counter] = Wire.read();
      counter++;
    }
    delay(10);
    address = address+30;
  }
}
void writeAddress(byte device_Address, int address, byte val)
{
  Wire.beginTransmission(device_Address);
  Wire.write((int)(address >> 8));   // MSB
  Wire.write((int)(address & 0xFF)); // LSB
  Wire.write(val);
//  for(int i=0;i<filenameLength;i++)
//  {
//    Wire.write(filename[i]);
//  }
  Wire.endTransmission();

  delay(5);
}
void writePattern(byte device_Address, int pattern, byte* data)
{
  int addr = pattern*512;//0 dla p1
  int counter = 0;
  for(int i=0;i<28;i++)//Zapisujemy dane w pamieci EEPROM etapami poniewaz kostka sie zapycha...
  {
    Wire.beginTransmission(device_Address);
    Wire.write((int)(addr >> 8));   // MSB
    Wire.write((int)(addr & 0xFF)); // LSB
    for(int j=0;j<16;j++)
    {
      Wire.write(data[counter]);
      counter++;
    }
    Wire.endTransmission();
    addr=addr+16;
    delay(10);
  }
}
