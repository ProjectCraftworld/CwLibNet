#!/usr/bin/env python3
"""
CwLibNet Migration Verification Script
=====================================

This script verifies the migration process and ensures that both projects
maintain their build integrity throughout the migration process.

Usage:
    python verify_migration.py [--verbose] [--fix-issues]
"""

import subprocess
import argparse
import logging
from pathlib import Path
from typing import Dict, List, Tuple, Optional

class BuildVerifier:
    """Handles build verification for CwLibNet projects."""
    
    def __init__(self, base_path: str):
        self.base_path = Path(base_path)
        self.logger = logging.getLogger(__name__)
        
    def run_build(self, project_path: Path) -> Tuple[bool, str]:
        """Run dotnet build on a project and return success status with output."""
        try:
            result = subprocess.run(
                ["dotnet", "build", str(project_path)],
                capture_output=True,
                text=True,
                cwd=project_path.parent
            )
            
            success = result.returncode == 0
            output = result.stdout + result.stderr
            
            return success, output
            
        except Exception as e:
            return False, f"Build execution failed: {e}"
    
    def verify_project_structure(self, project_path: Path) -> Dict[str, bool]:
        """Verify that project structure is intact."""
        checks = {}
        
        # Check .csproj file exists
        csproj_files = list(project_path.glob("*.csproj"))
        checks["csproj_exists"] = len(csproj_files) > 0
        
        # Check source directories exist
        expected_dirs = ["Enums", "Extensions", "IO", "Structs", "Types"]
        for dir_name in expected_dirs:
            dir_path = project_path / dir_name
            checks[f"dir_{dir_name.lower()}"] = dir_path.exists()
        
        # Check for C# files
        cs_files = list(project_path.glob("**/*.cs"))
        checks["has_cs_files"] = len(cs_files) > 0
        checks["cs_file_count"] = len(cs_files)
        
        return checks
    
    def verify_unity_compatibility(self, project_path: Path) -> Dict[str, bool]:
        """Verify Unity compatibility features."""
        checks = {}
        
        # Check for Unity bridge directory
        unity_bridge_path = project_path / "Unity" / "Bridges"
        checks["unity_bridges_exist"] = unity_bridge_path.exists()
        
        # Check for Unity bridge files
        if unity_bridge_path.exists():
            bridge_files = list(unity_bridge_path.glob("*.cs"))
            checks["unity_bridge_files"] = len(bridge_files) > 0
        else:
            checks["unity_bridge_files"] = False
        
        # Check project file for Unity references
        csproj_files = list(project_path.glob("*.csproj"))
        if csproj_files:
            with open(csproj_files[0], 'r') as f:
                csproj_content = f.read()
                checks["unity_conditional_refs"] = "UNITY_2021_3_OR_NEWER" in csproj_content
        else:
            checks["unity_conditional_refs"] = False
        
        return checks
    
    def run_verification(self) -> Dict[str, Dict]:
        """Run complete verification suite."""
        results = {}
        
        # Verify CwLibNet_4_HUB
        hub_path = self.base_path / "CwLibNet_4_HUB"
        if hub_path.exists():
            self.logger.info("Verifying CwLibNet_4_HUB...")
            
            # Structure verification
            structure_checks = self.verify_project_structure(hub_path)
            
            # Unity compatibility verification  
            unity_checks = self.verify_unity_compatibility(hub_path)
            
            # Build verification
            csproj_path = next(hub_path.glob("*.csproj"), None)
            if csproj_path:
                build_success, build_output = self.run_build(csproj_path)
            else:
                build_success, build_output = False, "No .csproj file found"
            
            results["CwLibNet_4_HUB"] = {
                "structure": structure_checks,
                "unity_compatibility": unity_checks,
                "build_success": build_success,
                "build_output": build_output
            }
        
        # Verify original CwLibNet for comparison
        cwlibnet_path = self.base_path / "CwLibNet"
        if cwlibnet_path.exists():
            self.logger.info("Verifying original CwLibNet...")
            
            structure_checks = self.verify_project_structure(cwlibnet_path)
            
            csproj_path = next(cwlibnet_path.glob("*.csproj"), None)
            if csproj_path:
                build_success, build_output = self.run_build(csproj_path)
            else:
                build_success, build_output = False, "No .csproj file found"
            
            results["CwLibNet"] = {
                "structure": structure_checks,
                "build_success": build_success,
                "build_output": build_output
            }
        
        return results
    
    def print_verification_report(self, results: Dict[str, Dict]):
        """Print a formatted verification report."""
        print("\n" + "="*60)
        print("CwLibNet Migration Verification Report")
        print("="*60)
        
        for project_name, project_results in results.items():
            print(f"\n📁 {project_name}")
            print("-" * 40)
            
            # Structure checks
            if "structure" in project_results:
                structure = project_results["structure"]
                print("📋 Structure:")
                print(f"  ✅ .csproj exists: {structure.get('csproj_exists', False)}")
                print(f"  ✅ C# files: {structure.get('cs_file_count', 0)} files")
                
                dirs = ["dir_enums", "dir_extensions", "dir_io", "dir_structs", "dir_types"]
                dir_status = all(structure.get(d, False) for d in dirs)
                print(f"  ✅ Required directories: {'✓' if dir_status else '✗'}")
            
            # Unity compatibility (only for _4_HUB)
            if "unity_compatibility" in project_results:
                unity = project_results["unity_compatibility"]
                print("🎮 Unity Compatibility:")
                print(f"  ✅ Unity bridges: {'✓' if unity.get('unity_bridges_exist', False) else '✗'}")
                print(f"  ✅ Bridge files: {'✓' if unity.get('unity_bridge_files', False) else '✗'}")
                print(f"  ✅ Conditional refs: {'✓' if unity.get('unity_conditional_refs', False) else '✗'}")
            
            # Build results
            build_success = project_results.get("build_success", False)
            print(f"🔨 Build Status: {'✅ SUCCESS' if build_success else '❌ FAILED'}")
            
            if not build_success:
                build_output = project_results.get("build_output", "")
                if build_output:
                    print("Build Output:")
                    for line in build_output.split('\n')[:10]:  # Show first 10 lines
                        if line.strip():
                            print(f"    {line}")
                    if len(build_output.split('\n')) > 10:
                        print("    ... (output truncated)")

def setup_logging(verbose: bool = False):
    """Setup logging configuration."""
    level = logging.DEBUG if verbose else logging.INFO
    logging.basicConfig(
        level=level,
        format='%(asctime)s - %(levelname)s - %(message)s'
    )

def main():
    """Main entry point."""
    parser = argparse.ArgumentParser(description='Verify CwLibNet migration status')
    parser.add_argument('--verbose', action='store_true', help='Enable verbose logging')
    parser.add_argument('--base-path', default='.', help='Base path to CwLibNet project')
    
    args = parser.parse_args()
    
    setup_logging(args.verbose)
    
    # Run verification
    verifier = BuildVerifier(args.base_path)
    results = verifier.run_verification()
    verifier.print_verification_report(results)
    
    # Return appropriate exit code
    all_builds_successful = all(
        result.get("build_success", False) 
        for result in results.values()
    )
    
    return 0 if all_builds_successful else 1

if __name__ == "__main__":
    exit(main())
