# PMP-Decrypter

This is a tool to decrypt the encrypted password strings in [Patch My PC](https://patchmypc.com) settings.xml files.  
In my case it was the SMTP password, but as there is only one encryption and decryption function in the tool, it should work for all of them.  
Patch My PC saves the values for the crypto functions in registry keys under:  
``HKLM\Software\Patch My PC Publishing Service``  
``Data1 = Key``  
``Data2 = Salt``  
``Data3 = IV``  

![image](https://github.com/LuemmelSec/PMP-Decrypter/assets/58529760/641e3174-96dd-4696-aaad-daff5fc1c25c)

I honestly dunno if this is also true for the latest latest latest version, but the one I stumbled upon seems to be from 04/2023 so pretty new.

The ACLs on those keys are lax, so can be read by everyone.

![image](https://github.com/LuemmelSec/PMP-Decrypter/assets/58529760/c9c76d58-5420-47a6-8443-3749a71439ef)

The settings file resides in the application's main folder and is called ``Settings.xml``.  

![image](https://github.com/LuemmelSec/PMP-Decrypter/assets/58529760/7189fe7c-a669-4a01-aa74-e71b8b248f02)

# Usage
The tool first trys to fetch those key values automagically for you.  
If not found, you still have the option to specify them manually. 

``PMP-Decrypter.exe <encryptedText> [privateKey] [saltKey] [ivKey]``  

![image](https://github.com/LuemmelSec/PMP-Decrypter/assets/58529760/e9bcb1f2-168c-4507-8d05-38a78159bd8e)

# Build
I used Visual Studio 2017, .Net Framework 4 and release mode.  
If you dare, you can use the precompiled version.

# Countermeasures

Honestly speaking: If a company which wants to connect to such critical assets takes security like this, I would not recommend using this at all.  
You could restrict access to the reg keys, so that not everyone is able to read them.

If you operate it, where ever possible use low privileged, dedicated accounts. Sending mails with your DA account is not the best idea you might have.  
