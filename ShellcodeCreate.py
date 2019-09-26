import donut
import argparse
import os
import base64


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument("-f", "--filename",
                        action="store",
                        help="input file")
    parser.add_argument("-o", "--output", action="store", help="output file")

    args = parser.parse_args()
    
    if not args.output or not args.filename:
        parser.print_help()
        exit()

    shellcode = donut.create(
        file=args.filename)
    
    payload = base64.b64encode(shellcode)
    
    text_file = open(args.output,'w')
    text_file.write(payload.decode('utf-8'))
    text_file.close
