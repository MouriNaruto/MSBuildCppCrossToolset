﻿using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections;
using Microsoft.Build.CPPTasks;
using Microsoft.Build.Framework;
// using Microsoft.Build.Linux.Shared;
// using Microsoft.Build.Linux.Tasks;
using Microsoft.Build.Utilities;
using System.Resources;

namespace YY.Build.Linux.Tasks.GCC
{
    public class Compile : Microsoft.Build.CPPTasks.VCToolTask
    {
        public Compile()
        {
            UseCommandProcessor = false;

            switchOrderList = new ArrayList();
            switchOrderList.Add("AlwaysAppend");
            switchOrderList.Add("CompileAs");
            switchOrderList.Add("Sources");
            switchOrderList.Add("BuildingInIde");
            switchOrderList.Add("AdditionalIncludeDirectories");
            switchOrderList.Add("DebugInformationFormat");
            switchOrderList.Add("ObjectFileName");
            switchOrderList.Add("WarningLevel");
            switchOrderList.Add("TreatWarningAsError");
            switchOrderList.Add("AdditionalWarning");
            switchOrderList.Add("Verbose");
            switchOrderList.Add("Optimization");
            switchOrderList.Add("StrictAliasing");
            switchOrderList.Add("UnrollLoops");
            switchOrderList.Add("LinkTimeOptimization");
            switchOrderList.Add("OmitFramePointers");
            switchOrderList.Add("NoCommonBlocks");
            switchOrderList.Add("PreprocessorDefinitions");
            switchOrderList.Add("UndefinePreprocessorDefinitions");
            switchOrderList.Add("UndefineAllPreprocessorDefinitions");
            switchOrderList.Add("ShowIncludes");
            switchOrderList.Add("PositionIndependentCode");
            switchOrderList.Add("ThreadSafeStatics");
            switchOrderList.Add("RelaxIEEE");
            switchOrderList.Add("HideInlineMethods");
            switchOrderList.Add("SymbolsHiddenByDefault");
            switchOrderList.Add("ExceptionHandling");
            switchOrderList.Add("RuntimeTypeInfo");
            switchOrderList.Add("CLanguageStandard");
            switchOrderList.Add("CppLanguageStandard");
            switchOrderList.Add("ForcedIncludeFiles");
            switchOrderList.Add("EnableASAN");
            switchOrderList.Add("AdditionalOptions");

            base.IgnoreUnknownSwitchValues = true;
        }
        
        private ArrayList switchOrderList;

        protected override ArrayList SwitchOrderList => switchOrderList;

        protected override string ToolName => "g++";

        protected override string AlwaysAppend => "-c";

        [Required]
        public ITaskItem[] Sources
        {
            get
            {
                if (IsPropertySet("Sources"))
                {
                    return base.ActiveToolSwitches["Sources"].TaskItemArray;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("Sources");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.ITaskItemArray);
                toolSwitch.Separator = " ";
                toolSwitch.Required = true;
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.TaskItemArray = value;
                base.ActiveToolSwitches.Add("Sources", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string[] AdditionalIncludeDirectories
        {
            get
            {
                if (IsPropertySet("AdditionalIncludeDirectories"))
                {
                    return base.ActiveToolSwitches["AdditionalIncludeDirectories"].StringList;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("AdditionalIncludeDirectories");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.StringPathArray);
                toolSwitch.DisplayName = "Additional Include Directories";
                toolSwitch.Description = "Specifies one or more directories to add to the include path; separate with semi-colons if more than one. (-I[path]).";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-I ";
                toolSwitch.Name = "AdditionalIncludeDirectories";
                toolSwitch.StringList = value;
                base.ActiveToolSwitches.Add("AdditionalIncludeDirectories", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public virtual string DebugInformationFormat
        {
            get
            {
                if (IsPropertySet("DebugInformationFormat"))
                {
                    return base.ActiveToolSwitches["DebugInformationFormat"].Value;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("DebugInformationFormat");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.String);
                toolSwitch.DisplayName = "Debug Information Format";
                toolSwitch.Description = "Specifies the type of debugging information generated by the compiler.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[3][]
                {
                    new string[2] { "None", "-g0" },
                    new string[2] { "Minimal", "-g1" },
                    new string[2] { "FullDebug", "-g2 -gdwarf-2" }
                };
                toolSwitch.SwitchValue = ReadSwitchMap("DebugInformationFormat", switchMap, value);
                toolSwitch.Name = "DebugInformationFormat";
                toolSwitch.Value = value;
                toolSwitch.MultipleValues = true;
                base.ActiveToolSwitches.Add("DebugInformationFormat", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string ObjectFileName
        {
            get
            {
                if (IsPropertySet("ObjectFileName"))
                {
                    return base.ActiveToolSwitches["ObjectFileName"].Value;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("ObjectFileName");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.File);
                toolSwitch.DisplayName = "Object File Name";
                toolSwitch.Description = "Specifies a name to override the default object file name; can be file or directory name. (/Fo[name]).";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-o ";
                toolSwitch.Name = "ObjectFileName";
                toolSwitch.Value = value;
                base.ActiveToolSwitches.Add("ObjectFileName", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string WarningLevel
        {
            get
            {
                if (IsPropertySet("WarningLevel"))
                {
                    return base.ActiveToolSwitches["WarningLevel"].Value;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("WarningLevel");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.String);
                toolSwitch.DisplayName = "Warning Level";
                toolSwitch.Description = "Select how strict you want the compiler to be about code errors.  Other flags should be added directly to Additional Options. (/w, /Weverything).";
                toolSwitch.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[2][]
                {
                    new string[2] { "TurnOffAllWarnings", "-w" },
                    new string[2] { "EnableAllWarnings", "-Wall" }
                };
                toolSwitch.SwitchValue = ReadSwitchMap("WarningLevel", switchMap, value);
                toolSwitch.Name = "WarningLevel";
                toolSwitch.Value = value;
                toolSwitch.MultipleValues = true;
                base.ActiveToolSwitches.Add("WarningLevel", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool TreatWarningAsError
        {
            get
            {
                if (IsPropertySet("TreatWarningAsError"))
                {
                    return base.ActiveToolSwitches["TreatWarningAsError"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("TreatWarningAsError");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Treat Warnings As Errors";
                toolSwitch.Description = "Treats all compiler warnings as errors. For a new project, it may be best to use /WX in all compilations; resolving all warnings will ensure the fewest possible hard-to-find code defects.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-Werror";
                toolSwitch.Name = "TreatWarningAsError";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("TreatWarningAsError", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string[] AdditionalWarning
        {
            get
            {
                if (IsPropertySet("AdditionalWarning"))
                {
                    return base.ActiveToolSwitches["AdditionalWarning"].StringList;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("AdditionalWarning");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.StringArray);
                toolSwitch.DisplayName = "Additional Warnings";
                toolSwitch.Description = "Defines a set of additional warning messages.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-W";
                toolSwitch.Name = "AdditionalWarning";
                toolSwitch.StringList = value;
                base.ActiveToolSwitches.Add("AdditionalWarning", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool Verbose
        {
            get
            {
                if (IsPropertySet("Verbose"))
                {
                    return base.ActiveToolSwitches["Verbose"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("Verbose");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Enable Verbose mode";
                toolSwitch.Description = "When Verbose mode is enabled, this tool would print out more information that for diagnosing the build.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-v";
                toolSwitch.Name = "Verbose";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("Verbose", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string Optimization
        {
            get
            {
                if (IsPropertySet("Optimization"))
                {
                    return base.ActiveToolSwitches["Optimization"].Value;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("Optimization");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.String);
                toolSwitch.DisplayName = "Optimization";
                toolSwitch.Description = "Specifies the optimization level for the application.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[5][]
                {
                    new string[2] { "Custom", "" },
                    new string[2] { "Disabled", "-O0" },
                    new string[2] { "MinSize", "-Os" },
                    new string[2] { "MaxSpeed", "-O2" },
                    new string[2] { "Full", "-O3" }
                };
                toolSwitch.SwitchValue = ReadSwitchMap("Optimization", switchMap, value);
                toolSwitch.Name = "Optimization";
                toolSwitch.Value = value;
                toolSwitch.MultipleValues = true;
                base.ActiveToolSwitches.Add("Optimization", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool StrictAliasing
        {
            get
            {
                if (IsPropertySet("StrictAliasing"))
                {
                    return base.ActiveToolSwitches["StrictAliasing"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("StrictAliasing");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Strict Aliasing";
                toolSwitch.Description = "Assume the strictest aliasing rules.  An object of one type will never be assumed to reside at the same address as an object of a different type.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-fstrict-aliasing";
                toolSwitch.ReverseSwitchValue = "-fno-strict-aliasing";
                toolSwitch.Name = "StrictAliasing";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("StrictAliasing", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool UnrollLoops
        {
            get
            {
                if (IsPropertySet("UnrollLoops"))
                {
                    return base.ActiveToolSwitches["UnrollLoops"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("UnrollLoops");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Unroll Loops";
                toolSwitch.Description = "Unroll loops to make application faster by reducing number of branches executed at the cost of larger code size.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-funroll-all-loops";
                toolSwitch.Name = "UnrollLoops";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("UnrollLoops", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool LinkTimeOptimization
        {
            get
            {
                if (IsPropertySet("LinkTimeOptimization"))
                {
                    return base.ActiveToolSwitches["LinkTimeOptimization"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("LinkTimeOptimization");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Link Time Optimization";
                toolSwitch.Description = "Enable Inter-Procedural optimizations by allowing the optimizer to look across object files in your application.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-flto";
                toolSwitch.Name = "LinkTimeOptimization";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("LinkTimeOptimization", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool OmitFramePointers
        {
            get
            {
                if (IsPropertySet("OmitFramePointers"))
                {
                    return base.ActiveToolSwitches["OmitFramePointers"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("OmitFramePointers");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Omit Frame Pointer";
                toolSwitch.Description = "Suppresses creation of frame pointers on the call stack.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-fomit-frame-pointer";
                toolSwitch.ReverseSwitchValue = "-fno-omit-frame-pointer";
                toolSwitch.Name = "OmitFramePointers";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("OmitFramePointers", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool NoCommonBlocks
        {
            get
            {
                if (IsPropertySet("NoCommonBlocks"))
                {
                    return base.ActiveToolSwitches["NoCommonBlocks"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("NoCommonBlocks");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "No Common Blocks";
                toolSwitch.Description = "Allocate even unintialized global variables in the data section of the object file, rather then generating them as common blocks";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-fno-common";
                toolSwitch.Name = "NoCommonBlocks";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("NoCommonBlocks", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string[] PreprocessorDefinitions
        {
            get
            {
                if (IsPropertySet("PreprocessorDefinitions"))
                {
                    return base.ActiveToolSwitches["PreprocessorDefinitions"].StringList;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("PreprocessorDefinitions");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.StringArray);
                toolSwitch.DisplayName = "Preprocessor Definitions";
                toolSwitch.Description = "Defines a preprocessing symbols for your source file. (-D)";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-D";
                toolSwitch.Name = "PreprocessorDefinitions";
                toolSwitch.StringList = value;
                base.ActiveToolSwitches.Add("PreprocessorDefinitions", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string[] UndefinePreprocessorDefinitions
        {
            get
            {
                if (IsPropertySet("UndefinePreprocessorDefinitions"))
                {
                    return base.ActiveToolSwitches["UndefinePreprocessorDefinitions"].StringList;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("UndefinePreprocessorDefinitions");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.StringArray);
                toolSwitch.DisplayName = "Undefine Preprocessor Definitions";
                toolSwitch.Description = "Specifies one or more preprocessor undefines.  (-U [macro])";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-U";
                toolSwitch.Name = "UndefinePreprocessorDefinitions";
                toolSwitch.StringList = value;
                base.ActiveToolSwitches.Add("UndefinePreprocessorDefinitions", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool UndefineAllPreprocessorDefinitions
        {
            get
            {
                if (IsPropertySet("UndefineAllPreprocessorDefinitions"))
                {
                    return base.ActiveToolSwitches["UndefineAllPreprocessorDefinitions"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("UndefineAllPreprocessorDefinitions");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Undefine All Preprocessor Definitions";
                toolSwitch.Description = "Undefine all previously defined preprocessor values.  (-undef)";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-undef";
                toolSwitch.Name = "UndefineAllPreprocessorDefinitions";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("UndefineAllPreprocessorDefinitions", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool ShowIncludes
        {
            get
            {
                if (IsPropertySet("ShowIncludes"))
                {
                    return base.ActiveToolSwitches["ShowIncludes"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("ShowIncludes");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Show Includes";
                toolSwitch.Description = "Generates a list of include files with compiler output.  (-H)";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-H";
                toolSwitch.Name = "ShowIncludes";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("ShowIncludes", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool PositionIndependentCode
        {
            get
            {
                if (IsPropertySet("PositionIndependentCode"))
                {
                    return base.ActiveToolSwitches["PositionIndependentCode"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("PositionIndependentCode");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Position Independent Code";
                toolSwitch.Description = "Generate Position Independent Code (PIC) for use in a shared library.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-fpic";
                toolSwitch.Name = "PositionIndependentCode";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("PositionIndependentCode", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool ThreadSafeStatics
        {
            get
            {
                if (IsPropertySet("ThreadSafeStatics"))
                {
                    return base.ActiveToolSwitches["ThreadSafeStatics"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("ThreadSafeStatics");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Statics are thread safe";
                toolSwitch.Description = "Emit Extra code to use routines specified in C++ ABI for thread safe initilization of local statics.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-fthreadsafe-statics";
                toolSwitch.ReverseSwitchValue = "-fno-threadsafe-statics";
                toolSwitch.Name = "ThreadSafeStatics";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("ThreadSafeStatics", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }
        public bool RelaxIEEE
        {
            get
            {
                if (IsPropertySet("RelaxIEEE"))
                {
                    return base.ActiveToolSwitches["RelaxIEEE"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("RelaxIEEE");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Floating Point Optimization";
                toolSwitch.Description = "Enables floating point optimizations by relaxing IEEE-754 compliance.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-ffast-math";
                toolSwitch.Name = "RelaxIEEE";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("RelaxIEEE", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool HideInlineMethods
        {
            get
            {
                if (IsPropertySet("HideInlineMethods"))
                {
                    return base.ActiveToolSwitches["HideInlineMethods"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("HideInlineMethods");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Inline Methods Hidden";
                toolSwitch.Description = "When enabled, out-of-line copies of inline methods are declared 'private extern'.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-fvisibility-inlines-hidden";
                toolSwitch.Name = "HideInlineMethods";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("HideInlineMethods", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool SymbolsHiddenByDefault
        {
            get
            {
                if (IsPropertySet("SymbolsHiddenByDefault"))
                {
                    return base.ActiveToolSwitches["SymbolsHiddenByDefault"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("SymbolsHiddenByDefault");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Symbol Hiddens By Default";
                toolSwitch.Description = "All symbols are declared 'private extern' unless explicitly marked to be exported using the '__attribute' macro.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-fvisibility=hidden";
                toolSwitch.Name = "SymbolsHiddenByDefault";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("SymbolsHiddenByDefault", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string ExceptionHandling
        {
            get
            {
                if (IsPropertySet("ExceptionHandling"))
                {
                    return base.ActiveToolSwitches["ExceptionHandling"].Value;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("ExceptionHandling");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.String);
                toolSwitch.DisplayName = "Enable C++ Exceptions";
                toolSwitch.Description = "Specifies the model of exception handling to be used by the compiler.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[2][]
                {
                    new string[2] { "Disabled", "-fno-exceptions" },
                    new string[2] { "Enabled", "-fexceptions" }
                };
                toolSwitch.SwitchValue = ReadSwitchMap("ExceptionHandling", switchMap, value);
                toolSwitch.Name = "ExceptionHandling";
                toolSwitch.Value = value;
                toolSwitch.MultipleValues = true;
                base.ActiveToolSwitches.Add("ExceptionHandling", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool RuntimeTypeInfo
        {
            get
            {
                if (IsPropertySet("RuntimeTypeInfo"))
                {
                    return base.ActiveToolSwitches["RuntimeTypeInfo"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("RuntimeTypeInfo");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Enable Run-Time Type Information";
                toolSwitch.Description = "Adds code for checking C++ object types at run time (runtime type information).     (frtti, fno-rtti)";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-frtti";
                toolSwitch.ReverseSwitchValue = "-fno-rtti";
                toolSwitch.Name = "RuntimeTypeInfo";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("RuntimeTypeInfo", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string CLanguageStandard
        {
            get
            {
                if (IsPropertySet("CLanguageStandard"))
                {
                    return base.ActiveToolSwitches["CLanguageStandard"].Value;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("CLanguageStandard");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.String);
                toolSwitch.DisplayName = "C Language Standard";
                toolSwitch.Description = "Determines the C language standard.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[8][]
                {
                    new string[2] { "Default", "" },
                    new string[2] { "c89", "-std=c89" },
                    new string[2] { "iso9899:199409", "-std=iso9899:199409" },
                    new string[2] { "c99", "-std=c99" },
                    new string[2] { "c11", "-std=c11" },
                    new string[2] { "gnu89", "-std=gnu89" },
                    new string[2] { "gnu99", "-std=gnu99" },
                    new string[2] { "gnu11", "-std=gnu11" }
                };
                toolSwitch.SwitchValue = ReadSwitchMap("CLanguageStandard", switchMap, value);
                toolSwitch.Name = "CLanguageStandard";
                toolSwitch.Value = value;
                toolSwitch.MultipleValues = true;
                base.ActiveToolSwitches.Add("CLanguageStandard", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public string CppLanguageStandard
	    {
		    get
		    {
			    if (IsPropertySet("CppLanguageStandard"))
			    {
				    return base.ActiveToolSwitches["CppLanguageStandard"].Value;
			    }
			    return null;
		    }
		    set
		    {
			    base.ActiveToolSwitches.Remove("CppLanguageStandard");
			    ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.String);
			    toolSwitch.DisplayName = "C++ Language Standard";
			    toolSwitch.Description = "Determines the C++ language standard.";
			    toolSwitch.ArgumentRelationList = new ArrayList();
			    string[][] switchMap = new string[17][]
			    {
				    new string[2] { "Default", "" },
				    new string[2] { "c++98", "-std=c++98" },
				    new string[2] { "c++03", "-std=c++03" },
				    new string[2] { "c++11", "-std=c++11" },
				    new string[2] { "c++1y", "-std=c++14" },
				    new string[2] { "c++14", "-std=c++14" },
				    new string[2] { "c++17", "-std=c++17" },
				    new string[2] { "c++2a", "-std=c++2a" },
				    new string[2] { "c++20", "-std=c++20" },
				    new string[2] { "gnu++98", "-std=gnu++98" },
				    new string[2] { "gnu++03", "-std=gnu++03" },
				    new string[2] { "gnu++11", "-std=gnu++11" },
				    new string[2] { "gnu++1y", "-std=gnu++1y" },
				    new string[2] { "gnu++14", "-std=gnu++14" },
				    new string[2] { "gnu++1z", "-std=gnu++1z" },
				    new string[2] { "gnu++17", "-std=gnu++17" },
				    new string[2] { "gnu++20", "-std=gnu++20" }
			    };
			    toolSwitch.SwitchValue = ReadSwitchMap("CppLanguageStandard", switchMap, value);
			    toolSwitch.Name = "CppLanguageStandard";
			    toolSwitch.Value = value;
			    toolSwitch.MultipleValues = true;
			    base.ActiveToolSwitches.Add("CppLanguageStandard", toolSwitch);
			    AddActiveSwitchToolValue(toolSwitch);
		    }
	    }

        public string CompileAs
        {
	        get
	        {
		        if (IsPropertySet("CompileAs"))
		        {
			        return base.ActiveToolSwitches["CompileAs"].Value;
		        }
		        return null;
	        }
	        set
	        {
		        base.ActiveToolSwitches.Remove("CompileAs");
		        ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.String);
		        toolSwitch.DisplayName = "Compile As";
		        toolSwitch.Description = "Select compile language option for .c and .cpp files.  'Default' will detect based on .c or .cpp extention. (-x c, -x c++)";
		        toolSwitch.ArgumentRelationList = new ArrayList();
		        string[][] switchMap = new string[3][]
		        {
			        new string[2] { "Default", "" },
			        new string[2] { "CompileAsC", "-x c" },
			        new string[2] { "CompileAsCpp", "-x c++" }
		        };
		        toolSwitch.SwitchValue = ReadSwitchMap("CompileAs", switchMap, value);
		        toolSwitch.Name = "CompileAs";
		        toolSwitch.Value = value;
		        toolSwitch.MultipleValues = true;
		        base.ActiveToolSwitches.Add("CompileAs", toolSwitch);
		        AddActiveSwitchToolValue(toolSwitch);
	        }
        }

        public string[] ForcedIncludeFiles
        {
            get
            {
                if (IsPropertySet("ForcedIncludeFiles"))
                {
                    return base.ActiveToolSwitches["ForcedIncludeFiles"].StringList;
                }
                return null;
            }
            set
            {
                base.ActiveToolSwitches.Remove("ForcedIncludeFiles");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.StringPathArray);
                toolSwitch.DisplayName = "Forced Include Files";
                toolSwitch.Description = "one or more forced include files.     (-include [name])";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-include ";
                toolSwitch.Name = "ForcedIncludeFiles";
                toolSwitch.StringList = value;
                base.ActiveToolSwitches.Add("ForcedIncludeFiles", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public bool EnableASAN
        {
            get
            {
                if (IsPropertySet("EnableASAN"))
                {
                    return base.ActiveToolSwitches["EnableASAN"].BooleanValue;
                }
                return false;
            }
            set
            {
                base.ActiveToolSwitches.Remove("EnableASAN");
                ToolSwitch toolSwitch = new ToolSwitch(ToolSwitchType.Boolean);
                toolSwitch.DisplayName = "Enable Address Sanitizer";
                toolSwitch.Description = "Compiles program with AddressSanitizer. Compile with -fno-omit-frame-pointer and compiler optimization level -Os or -O0 for best results. Must link with Address Sanitizer option too. Must run with debugger to view diagnostic results.";
                toolSwitch.ArgumentRelationList = new ArrayList();
                toolSwitch.SwitchValue = "-fsanitize=address";
                toolSwitch.Name = "EnableASAN";
                toolSwitch.BooleanValue = value;
                base.ActiveToolSwitches.Add("EnableASAN", toolSwitch);
                AddActiveSwitchToolValue(toolSwitch);
            }
        }

        public override bool Execute()
        {
            foreach (var Item in Sources)
            {
                Log.LogMessage(MessageImportance.High, Item.ItemSpec);
            }

            return base.Execute();
        }
    }
}