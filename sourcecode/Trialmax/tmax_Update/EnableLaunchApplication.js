// EnableLaaunchApplication.js <msi-file>
// Performs a post-build fixup of an msi to launch a specific file when the install has completed


// Configurable values
var checkboxChecked = true;			// Is the checkbox on the finished dialog checked by default?
var checkboxText = "Launch setup for the following other components on clicking \"Close\"";	// Text for the checkbox on the finished dialog
var itemListText = "- Crystal Reports Engine";	// Text for the checkbox on the finished dialog
var itemWMEncoderText = "- KLite Codec";
var filename = "TmaxUtilityInsatller.exe";	// The name of the executable to launch - change this to match the file you want to launch at the end of your setup


// Constant values from Windows Installer
var msiOpenDatabaseModeTransact = 1;

var msiViewModifyInsert         = 1
var msiViewModifyUpdate         = 2
var msiViewModifyAssign         = 3
var msiViewModifyReplace        = 4
var msiViewModifyDelete         = 6



if (WScript.Arguments.Length != 1)
{
    WScript.StdErr.WriteLine(WScript.ScriptName + " file");
    WScript.Quit(1);
}

var filespec = WScript.Arguments(0);
var installer = WScript.CreateObject("WindowsInstaller.Installer");
var database = installer.OpenDatabase(filespec, msiOpenDatabaseModeTransact);

var sql
var view
var record

try
{
    var fileId = FindFileIdentifier(database, filename);
    if (!fileId)
        throw "Unable to find '" + filename + "' in File table";

    // Modify the text on the finish form
    sql = "UPDATE `Control` SET `Text`='{\\VSI_MS_Sans_Serif13.0_0_0} [ProductName] has been successfully installed.' WHERE `Dialog_`='FinishedForm' AND `Control`='BodyText'";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();

    WScript.Echo("Updating the Control table...");
    // Modify the Control_Next of BannerBmp control to point to the new CheckBox
    sql = "SELECT `Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help` FROM `Control` WHERE `Dialog_`='FinishedForm' AND `Control`='BannerBmp'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.StringData(11) = "CheckboxLaunch";
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    // Resize the BodyText and BodyTextRemove controls to be reasonable
    sql = "SELECT `Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help` FROM `Control` WHERE `Dialog_`='FinishedForm' AND `Control`='BodyTextRemove'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.IntegerData(7) = 33;
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    sql = "SELECT `Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help` FROM `Control` WHERE `Dialog_`='FinishedForm' AND `Control`='BodyText'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.IntegerData(7) = 33;
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    // Insert the new CheckBox control
    sql = "INSERT INTO `Control` (`Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help`) VALUES ('FinishedForm', 'CheckboxLaunch', 'CheckBox', '18', '117', '343', '12', '3', 'LAUNCHAPP', '{\\VSI_MS_Sans_Serif13.0_0_0}" + checkboxText + "', 'ItemList', '|')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();

    // Modify the Control_Next of BannerBmp control to point to the new CheckBox
    sql = "SELECT `Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help` FROM `Control` WHERE `Dialog_`='FinishedForm' AND `Control`='CheckboxLaunch'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.StringData(11) = "ItemList";
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    // Insert the new CheckBox control
    sql = "INSERT INTO `Control` (`Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help`) VALUES ('FinishedForm', 'ItemList', 'Text', '25', '142', '343', '12', '3', 'LAUNCHAPP', '{\\VSI_MS_Sans_Serif13.0_0_0}" + itemListText + "', 'ItemWMEncoder', '|')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();


    // Modify the Control_Next of BannerBmp control to point to the new CheckBox
    sql = "SELECT `Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help` FROM `Control` WHERE `Dialog_`='FinishedForm' AND `Control`='ItemList'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.StringData(11) = "ItemWMEncoder";
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    // Insert the new CheckBox control
    sql = "INSERT INTO `Control` (`Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help`) VALUES ('FinishedForm', 'ItemWMEncoder', 'Text', '25', '162', '343', '12', '3', 'LAUNCHAPP', '{\\VSI_MS_Sans_Serif13.0_0_0}" + itemWMEncoderText + "', 'CloseButton', '|')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();


    WScript.Echo("Updating the ControlEvent table...");
    // Modify the Order of the EndDialog event of the FinishedForm to 1
    sql = "SELECT `Dialog_`, `Control_`, `Event`, `Argument`, `Condition`, `Ordering` FROM `ControlEvent` WHERE `Dialog_`='FinishedForm' AND `Event`='EndDialog'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.IntegerData(6) = 1;
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    // Insert the Event to launch the application
    sql = "INSERT INTO `ControlEvent` (`Dialog_`, `Control_`, `Event`, `Argument`, `Condition`, `Ordering`) VALUES ('FinishedForm', 'CloseButton', 'DoAction', 'VSDCA_Launch', 'LAUNCHAPP=1', '0')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();



    WScript.Echo("Updating the CustomAction table...");
    // Insert the custom action to launch the application when finished
    sql = "INSERT INTO `CustomAction` (`Action`, `Type`, `Source`, `Target`) VALUES ('VSDCA_Launch', '210', '" + fileId + "', '')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();



    if (checkboxChecked)
    {
        WScript.Echo("Updating the Property table...");
        // Set the default value of the CheckBox
        sql = "INSERT INTO `Property` (`Property`, `Value`) VALUES ('LAUNCHAPP', '1')";
        view = database.OpenView(sql);
        view.Execute();
        view.Close();
    }

    // NEW - START
    WScript.Echo("Updating the ControlCondition table...");
    // Insert the conditions where the Launch Application Checkbox appears
    sql = "INSERT INTO `ControlCondition` (`Dialog_`, `Control_`, `Action`, `Condition`) VALUES ('FinishedForm', 'CheckboxLaunch', 'Show', 'REMOVE=\"\"')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();

    sql = "INSERT INTO `ControlCondition` (`Dialog_`, `Control_`, `Action`, `Condition`) VALUES ('FinishedForm', 'CheckboxLaunch', 'Hide', 'REMOVE<>\"\"')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();
    //NEW - END

    // NEW - START
    WScript.Echo("Updating the ControlCondition table...");
    // Insert the conditions where the Launch Application Checkbox appears
    sql = "INSERT INTO `ControlCondition` (`Dialog_`, `Control_`, `Action`, `Condition`) VALUES ('FinishedForm', 'ItemList', 'Show', 'REMOVE=\"\"')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();

    sql = "INSERT INTO `ControlCondition` (`Dialog_`, `Control_`, `Action`, `Condition`) VALUES ('FinishedForm', 'ItemList', 'Hide', 'REMOVE<>\"\"')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();
    //NEW - END


    database.Commit();
}
catch(e)
{
    WScript.StdErr.WriteLine(e);
    WScript.Quit(1);
}



function FindFileIdentifier(database, fileName)
{
    var sql
    var view
    var record

    // First, try to find the exact file name
    sql = "SELECT `File` FROM `File` WHERE `FileName`='" + fileName + "'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    if (record)
    {
        var value = record.StringData(1);
        view.Close();
        return value;
    }
    view.Close();

    // The file may be in SFN|LFN format.  Look for a filename in this case next
    sql = "SELECT `File`, `FileName` FROM `File`";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    while (record)
    {
        if (StringEndsWith(record.StringData(2), "|" + fileName))
        {
            var value = record.StringData(1);
            view.Close();
            return value;
        }

        record = view.Fetch();
    }
    view.Close();

}

function StringEndsWith(str, value)
{
    if (str.length < value.length)
        return false;

    return (str.indexOf(value, str.length - value.length) != -1);
}
