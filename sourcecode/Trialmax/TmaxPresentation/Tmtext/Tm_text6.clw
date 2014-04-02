; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
LastClass=CTMTextCtrl
LastTemplate=CDialog
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "Tm_text6.h"
CDK=Y

ClassCount=2
Class1=CTMTextCtrl
Class2=CTMTextProperties

ResourceCount=2
Resource1=IDD_ABOUTBOX_TMTEXT
LastPage=0
Resource2=IDD_PROPPAGE_TMTEXT

[CLS:CTMTextCtrl]
Type=0
HeaderFile=include\tmtext.h
ImplementationFile=source\tmtext.cpp
BaseClass=COleControl
Filter=W
VirtualFilter=wWC
LastObject=CTMTextCtrl

[CLS:CTMTextProperties]
Type=0
HeaderFile=include\tmtextpg.h
ImplementationFile=source\tmtextpg.cpp
BaseClass=COlePropertyPage

[DLG:IDD_ABOUTBOX_TMTEXT]
Type=1
Class=?
ControlCount=4
Control1=IDC_STATIC,static,1342177283
Control2=IDC_STATIC,static,1342308352
Control3=IDC_STATIC,static,1342308352
Control4=IDOK,button,1342373889

[DLG:IDD_PROPPAGE_TMTEXT]
Type=1
Class=CTMTextProperties
ControlCount=1
Control1=IDC_STATIC,static,1342308353

