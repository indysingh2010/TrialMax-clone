# TrialMax
TrialMax Presentation Software

TrialMax Source Code Setup Guide for Developers:
--------------------------------------------------------------------------------------
<b>Prerequisite:</b> <br>
<ol>
<li>Visual Studio 2010 Service Pack 1 with NuGet Package Manager</li>
<li>TrialMax 7 (preferably latest version)</li>
</ol>

<b>Steps for source code setup:</b><br>
<ol>
<li>Execute command in git bash: <i>git clone https://github.com/cpaulin/TrialMax.git</i></li>
<li>Open "TrialMax\TrialMaxDependencies\TrialMaxDependencies.sln" file in Visual Studio 2010.</li>
<li>Build the project "TrialMaxDependencies".</li>
<li>Execute the TrialMaxDependencies application by either pressing F5(Start Debugging) in Visual Studio or directly run the TrialMaxDependencies.exe from the location "TrialMax\TrialMaxDependencies\TrialMaxDependencies\bin\Debug\".</li>
<li>Open "TrialMax\sourcecode\Trialmax\TmaxManager_all.sln" in Visual Studio 2010. Clean the solution TmaxManager_all and then build.</li>
</ol>
