#!/usr/bin/env python3

import os
import re

def remove_unnecessary_using_statements():
    """Remove unnecessary using static statements from enum files."""
    
    cwlibnet_path = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    
    # Pattern to match the using static statement
    using_pattern = r'using static CwLibNet\.Serializer;\s*\n'
    
    files_fixed = 0
    
    # Find all .cs files in Enums directory
    enums_path = os.path.join(cwlibnet_path, "Enums")
    
    for file in os.listdir(enums_path):
        if file.endswith('.cs'):
            file_path = os.path.join(enums_path, file)
            
            try:
                # Read the file
                with open(file_path, 'r', encoding='utf-8') as f:
                    content = f.read()
                
                # Check if it's a pure enum file (no serialization methods)
                # Look for signs it's just an enum definition
                if ('public enum ' in content and 
                    'Serialize(' not in content and 
                    'serializer.' not in content and
                    re.search(using_pattern, content)):
                    
                    # Remove the using static statement
                    new_content = re.sub(using_pattern, '', content)
                    
                    # Write back the file
                    with open(file_path, 'w', encoding='utf-8') as f:
                        f.write(new_content)
                    
                    files_fixed += 1
                    print(f"Removed using from: {file_path}")
            
            except Exception as e:
                print(f"Error processing {file_path}: {e}")
    
    print(f"\nFixed {files_fixed} enum files.")

if __name__ == "__main__":
    remove_unnecessary_using_statements()
