## WixVersionControl 项目介绍

### 用于帮我们将WPF、MFC项目的Version与WIX项目的Version关联，实现自动化�


## WPF使用

- Generated event   
```
@echo $(ProjectName) move files
if $(PlatformTarget)==x86 (
if exist "$(ProjectDir)$(OutDir)WPF_Sample.exe" (
xcopy "$(ProjectDir)$(OutDir)WPF_Sample.exe" "$(SolutionDir)WPF_Setup\SourceFiles\" /y 
)
)

@echo update setup
$(ProjectDir)Script\WixVersionControl.exe WPF $(SolutionDir)WPF_Sample\Properties\AssemblyInfo.cs $(SolutionDir)WPF_Setup\Product.wxs $(SolutionDir)WPF_Setup\WPF_Setup.wixproj
```

## MFC使用
- Generated event   
```
@echo $(ProjectName) move files
if $(PlatformTarget)==x86 (
if exist "$(SolutionDir)$(Configuration)\MFC_Sample.exe" (
xcopy "$(SolutionDir)$(Configuration)\MFC_Sample.exe" "$(SolutionDir)MFC_Setup\SourceFiles\" /y 
)
)

@echo update setup
$(ProjectDir)Script\WixVersionControl.exe MFC $(SolutionDir)MFC_Sample\MFCSample.rc $(SolutionDir)MFC_Setup\Product.wxs $(SolutionDir)MFC_Setup\MFC_Setup.wixproj
```
[WixVersionControl Wix项目版本控制](https://www.swack.cn/wiki/001565675133949eff0d3d5a51f48288cf6d8248905e28f000/0015698267883461a63908e92ad428c8aefc24d7f25bb46000)