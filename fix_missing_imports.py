#!/usr/bin/env python3
import os
import re

# Common missing types and their namespaces
TYPE_NAMESPACES = {
    'SlotID': 'CwLibNet.Structs.Slot',
    'Slot': 'CwLibNet.Structs.Slot',
    'InventoryItem': 'CwLibNet.Structs.Profile',
    'DataLabel': 'CwLibNet.Types.Data',
    'Challenge': 'CwLibNet.Structs.Profile',
    'Treasure': 'CwLibNet.Structs.Profile',
    'StringLookupTable': 'CwLibNet.Types.Data',
    'MaterialWire': 'CwLibNet.Structs.Gmat',
    'MaterialBox': 'CwLibNet.Structs.Gmat',
    'Thing': 'CwLibNet.Structs.Things',
    'Serializer': 'CwLibNet.IO.Serializer'
}

def fix_missing_imports(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Find existing using statements
    using_pattern = r'^using ([^;]+);'
    existing_usings = set()
    for match in re.finditer(using_pattern, content, re.MULTILINE):
        using_line = match.group(1).strip()
        if not using_line.startswith('static '):
            existing_usings.add(using_line)
    
    # Find which types are used but not imported
    missing_imports = set()
    for type_name, namespace in TYPE_NAMESPACES.items():
        if type_name in content and namespace not in existing_usings:
            missing_imports.add(namespace)
    
    if not missing_imports:
        return False
    
    # Find insertion point (after last non-static using)
    using_matches = re.findall(using_pattern, content, re.MULTILINE)
    if not using_matches:
        return False
    
    # Find the last non-static using statement
    last_using_line = None
    for using_line in using_matches:
        if not using_line.startswith('static '):
            last_using_line = f"using {using_line};"
    
    if not last_using_line:
        last_using_line = f"using {using_matches[0]};"
    
    # Insert new imports after the last using statement
    insertion_point = content.find(last_using_line) + len(last_using_line)
    
    new_imports = '\n'.join(f"using {namespace};" for namespace in sorted(missing_imports))
    new_content = content[:insertion_point] + '\n' + new_imports + content[insertion_point:]
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(new_content)
    
    print(f"Fixed missing imports in {file_path}: {', '.join(sorted(missing_imports))}")
    return True

def scan_directory(directory):
    fixed_count = 0
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cs'):
                file_path = os.path.join(root, file)
                if fix_missing_imports(file_path):
                    fixed_count += 1
    return fixed_count

if __name__ == "__main__":
    directory = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    fixed = scan_directory(directory)
    print(f"Fixed {fixed} files")
