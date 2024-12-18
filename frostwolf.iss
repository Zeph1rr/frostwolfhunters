; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "FrostWolfHunters"
;#define MyAppVersion "1.0"
#define MyAppPublisher "Zeph1rr, inc."
#define MyAppURL "https://github.com/Zeph1rr/frostwolfhunters"
#define MyAppExeName "FrostwolfHunters.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{D4734D06-E7BB-4AF4-962F-96E5CD43A8F0}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile={#MySourcePath}\LICENSE   
OutputDir={#MyOutputDir}
OutputBaseFilename=frostwolfhunters-setup-{#MyAppVersion}
SetupIconFile={#MySourcePath}\assets\images\icon.ico
Password={#MyPassword}
Encryption=yes
Compression=lzma
SolidCompression=yes
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl";
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#MySourcePath}\build\FrostwolfHunters.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MySourcePath}\build\UnityCrashHandler64.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MySourcePath}\build\UnityPlayer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MySourcePath}\build\FrostwolfHunters_Data\*"; DestDir: "{app}\FrostwolfHunters_Data"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MySourcePath}\build\MonoBleedingEdge\*"; DestDir: "{app}\MonoBleedingEdge"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
