#!/usr/bin/env python3

import os
import re
import subprocess
from pathlib import Path

def find_literal_ref_out_issues():
    """Find all files with literal ref/out parameter issues."""
    result = subprocess.run([
        'grep', '-r', '-n', '-E', r'ref [0-9]|ref true|ref false|out [0-9]|out true|out false', 
        '/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB/', '--include=*.cs'
    ], capture_output=True, text=True)
    
    issues = []
    for line in result.stdout.splitlines():
        if ':' in line:
            file_path, line_num, content = line.split(':', 2)
            issues.append((file_path, int(line_num), content.strip()))
    
    return issues

def fix_literal_refs_in_file(file_path, issues):
    """Fix literal ref/out parameters in a specific file."""
    print(f"Fixing {file_path}...")
    
    with open(file_path, 'r') as f:
        lines = f.readlines()
    
    changes_made = False
    
    # Process issues in reverse order to maintain line numbers
    for line_num, content in sorted([(issue[1], issue[2]) for issue in issues if issue[0] == file_path], reverse=True):
        line_idx = line_num - 1
        
        if line_idx < len(lines):
            original_line = lines[line_idx]
            
            # Replace literal ref/out parameters
            # ref 0 -> ref temp_int
            new_line = re.sub(r'\bref 0\b', 'ref temp_int', original_line)
            # ref true -> ref temp_bool_true  
            new_line = re.sub(r'\bref true\b', 'ref temp_bool_true', new_line)
            # ref false -> ref temp_bool_false
            new_line = re.sub(r'\bref false\b', 'ref temp_bool_false', new_line)
            # out 0 -> out temp_int
            new_line = re.sub(r'\bout 0\b', 'out temp_int', new_line)
            # out true -> out temp_bool_true
            new_line = re.sub(r'\bout true\b', 'out temp_bool_true', new_line)
            # out false -> out temp_bool_false
            new_line = re.sub(r'\bout false\b', 'out temp_bool_false', new_line)
            
            if new_line != original_line:
                changes_made = True
                lines[line_idx] = new_line
                print(f"  Line {line_num}: {original_line.strip()} -> {new_line.strip()}")
    
    if changes_made:
        # Add temp variables at the beginning of the method
        # Find the method start
        for i, line in enumerate(lines):
            if 'void Serialize(' in line or 'object Serialize(' in line:
                # Find the opening brace
                for j in range(i, min(i + 10, len(lines))):
                    if '{' in lines[j]:
                        # Insert temp variables after the opening brace
                        temp_vars = [
                            "        int temp_int = 0;\n",
                            "        bool temp_bool_true = true;\n", 
                            "        bool temp_bool_false = false;\n",
                            "\n"
                        ]
                        lines[j+1:j+1] = temp_vars
                        break
                break
        
        with open(file_path, 'w') as f:
            f.writelines(lines)
        print(f"  Fixed {file_path}")
    else:
        print(f"  No changes needed for {file_path}")

def main():
    print("Finding literal ref/out parameter issues...")
    issues = find_literal_ref_out_issues()
    
    if not issues:
        print("No literal ref/out issues found!")
        return
    
    print(f"Found {len(issues)} issues across multiple files")
    
    # Group by file
    files_with_issues = {}
    for file_path, line_num, content in issues:
        if file_path not in files_with_issues:
            files_with_issues[file_path] = []
        files_with_issues[file_path].append((line_num, content))
    
    print(f"Files to fix: {len(files_with_issues)}")
    
    for file_path in files_with_issues:
        fix_literal_refs_in_file(file_path, issues)
    
    print("Done fixing literal ref/out parameters!")

if __name__ == "__main__":
    main()
