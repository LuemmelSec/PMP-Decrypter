# PMP-Decrypter

This is a tool to decrypt the encrypted password strings in Patch My PC settings.xml files.  
In my case it was the SMTP password, but as there is only one encryption and decryption function in the tool, it should work for all of them.  
Patch My PC saves the values for the crypto functions in registry keys under:  
``HKLM\Software\Patch My PC Publishing Service``  
``Data1 = Key``  
``Data2 = Salt``  
``Data3 = IV``  

![image](https://github.com/LuemmelSec/PMP-Decrypter/assets/58529760/641e3174-96dd-4696-aaad-daff5fc1c25c)

The settings file resides in the application's main folder and is called ``Settings.xml``.  

![image](https://github.com/LuemmelSec/PMP-Decrypter/assets/58529760/7189fe7c-a669-4a01-aa74-e71b8b248f02)

# Usage
The tool first trys to fetch those key values automagically for you.  
If not found, you still have the option to specify them manually. 

``PMP-Decrypter.exe <encryptedText> [privateKey] [saltKey] [ivKey]``  

![image](https://github.com/LuemmelSec/PMP-Decrypter/assets/58529760/e9bcb1f2-168c-4507-8d05-38a78159bd8e)

# Build
I used Visual Studi 2017, .Net Framework 4 and release mode.  
If you dare, you can use the precompiled version.
