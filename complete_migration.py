#!/usr/bin/env python3

import os
import re
import sys

def complete_static_migration(file_path):
    """Complete the migration to static Serializer API"""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        changes_made = []
        
        # Skip if this is the main Serializer class definition
        if 'class Serializer' in content and 'namespace CwLibNet.IO.Serializer' in content:
            return False, []
        
        # Add static import if not present and Serializer is used
        if 'Serializer.' in content and 'using static net.torutheredfox.craftworld.serialization.Serializer;' not in content:
            # Find the using statements section
            lines = content.splitlines()
            using_insert_pos = -1
            
            for i, line in enumerate(lines):
                if line.strip().startswith('using ') and not line.strip().startswith('using static'):
                    using_insert_pos = i + 1
                elif line.strip().startswith('namespace ') or line.strip().startswith('public ') or line.strip().startswith('internal '):
                    break
            
            if using_insert_pos > -1:
                lines.insert(using_insert_pos, 'using static net.torutheredfox.craftworld.serialization.Serializer;')
                content = '\n'.join(lines)
                changes_made.append("Added static Serializer import")
        
        # Pattern 1: Replace instance serializer creation with static context setup
        # This is complex - for now, let's focus on the method calls
        
        # Pattern 2: Replace serializer.Reference() calls with static Reference()
        reference_pattern = r'\bserializer\.Reference\('
        if re.search(reference_pattern, content):
            content = re.sub(reference_pattern, 'Reference(', content)
            changes_made.append("Converted serializer.Reference() to static Reference()")
        
        # Pattern 3: Replace serializer.GetBuffer() calls with static GetBuffer()
        getbuffer_pattern = r'\bserializer\.GetBuffer\(\)'
        if re.search(getbuffer_pattern, content):
            content = re.sub(getbuffer_pattern, 'GetBuffer()', content)
            changes_made.append("Converted serializer.GetBuffer() to static GetBuffer()")
        
        # Pattern 4: Replace serializer.GetDependencies() calls with static GetDependencies()
        getdeps_pattern = r'\bserializer\.GetDependencies\(\)'
        if re.search(getdeps_pattern, content):
            content = re.sub(getdeps_pattern, 'GetDependencies()', content)
            changes_made.append("Converted serializer.GetDependencies() to static GetDependencies()")
        
        # Pattern 5: Replace other common serializer instance methods
        common_methods = [
            ('serializer\.I16\(', 'GetOutput().I16(' if 'IsWriting()' in content else 'GetInput().I16('),
            ('serializer\.V4\(', 'GetOutput().V4(' if 'IsWriting()' in content else 'GetInput().V4('),
            ('serializer\.I32\(', 'GetOutput().I32(' if 'IsWriting()' in content else 'GetInput().I32('),
            ('serializer\.GetRevision\(\)', 'GetRevision()'),
            ('serializer\.IsWriting\(\)', 'IsWriting()'),
        ]
        
        for pattern, replacement in common_methods:
            if re.search(pattern, content):
                content = re.sub(pattern, replacement, content)
                changes_made.append(f"Converted {pattern} to static {replacement}")
        
        # Pattern 6: Handle complex cases with conditional logic
        # Look for patterns like: serializer.Method(args) where we need context
        
        # Write the file if changes were made
        if content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            return True, changes_made
        else:
            return False, []
            
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False, []

def find_files_needing_migration():
    """Find files that still have instance-based serializer calls"""
    files_needing_migration = []
    
    cwlibnet_hub_dir = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    
    for root, dirs, files in os.walk(cwlibnet_hub_dir):
        for file in files:
            if file.endswith('.cs'):
                file_path = os.path.join(root, file)
                try:
                    with open(file_path, 'r', encoding='utf-8') as f:
                        content = f.read()
                    
                    # Check for instance-based calls
                    if any(pattern in content for pattern in [
                        'serializer.Reference(',
                        'serializer.GetBuffer(',
                        'serializer.GetDependencies(',
                        'var serializer = new Serializer(',
                        'Serializer serializer = new(',
                        'serializer.I16(',
                        'serializer.V4(',
                        'serializer.I32(',
                        'serializer.GetRevision(',
                        'serializer.IsWriting('
                    ]):
                        files_needing_migration.append(file_path)
                        
                except Exception as e:
                    print(f"Error reading {file_path}: {e}")
    
    return files_needing_migration

def main():
    print("Completing static Serializer migration...")
    
    files_to_migrate = find_files_needing_migration()
    
    if not files_to_migrate:
        print("No files found needing migration!")
        return
    
    print(f"Found {len(files_to_migrate)} files needing migration:")
    for file_path in files_to_migrate:
        print(f"  {file_path}")
    
    total_files_changed = 0
    total_changes = 0
    
    for file_path in files_to_migrate:
        changed, changes = complete_static_migration(file_path)
        if changed:
            total_files_changed += 1
            total_changes += len(changes)
            print(f"\n✓ {file_path}")
            for change in changes:
                print(f"  - {change}")
    
    print(f"\n=== Migration Summary ===")
    print(f"Files changed: {total_files_changed}")
    print(f"Total changes: {total_changes}")
    
    if total_files_changed > 0:
        print("\n⚠️  Note: This script handles basic conversions.")
        print("   Complex instance serializer usage may need manual review.")
        print("   Please check the files and test the build.")

if __name__ == "__main__":
    main()
