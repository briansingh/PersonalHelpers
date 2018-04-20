[CmdletBinding()]
param
(
    [Parameter(Mandatory=$True, Position=1)]
    [string]$dbName

    # This switch will only simulate the action
    ,
    [switch]$WhatIf 

	# This switch will clear the methods table
	,
	[switch]$ClearMethodsCache
)

# Retrieves the custom functions for a particular database from the main Registry database
# load the data into the local database
# This is a complete unload and reload of the data

$ServerName = ".\SQL2012"
$DatabaseName = "Registry1_100"
$basePath = "C:\Users\Medidata"

function executeQuery($Query) 
{
    #Timeout parameters
    $QueryTimeout = 120
    $ConnectionTimeout = 30

    #Action of connecting to the Database and executing the query and returning results if there were any.
    $conn=New-Object System.Data.SqlClient.SQLConnection
    $ConnectionString = "Server={0};Database={1};Integrated Security=True;Connect Timeout={2}" -f $ServerName,$DatabaseName,$ConnectionTimeout
    $conn.ConnectionString=$ConnectionString
    $conn.Open()
    $cmd=New-Object system.Data.SqlClient.SqlCommand($Query,$conn)
    $cmd.CommandTimeout=$QueryTimeout
	$cmd.ExecuteNonQuery()
    $conn.Close()
}

function clearExistingData($dbName)
{
	Write-Host "`nClearing existing data for $dbName" -foregroundcolor Yellow
	$rowCount = executeQuery("exec ClearExistingData '$dbName'");

	# "$rowCount rows deleted`n"
}

function fetchData($dbName) 
{
	Write-Host "Fetching the CustomFunctions for Database: $dbName" -foregroundColor Yellow
	bcp "select * from CustomFunctions where DB = '$($dbName)'" queryout "$($basePath)\CustomFunctions.$dbName.data" -n -S hdc64rndSQL02.hdc.mdsol.com -U rave -P rave -d Registry1
	if (test-path -path "$($basePath)\CustomFunctions.$dbName.data")
	{
		bcp "CustomFunctions" in "$($basePath)\CustomFunctions.$dbName.data" -n -S .\SQL2012 -T -d $DatabaseName
	}
	else
	{
		Write-host "Unable to find $($basePath)\CustomFunctions.$($dbName).data" -foregroundcolor red
	}
}

function runCodeAnalysis($dbName) 
{
	Write-Host "`nPerforming Source Code Analysis for $dbName" -foregroundcolor yellow
	& "C:\Users\Medidata\Documents\visual studio 2013\Projects\CustomFunctionAnalysis_003\CustomFunctionAnalysis_003\bin\Debug\CustomFunctionAnalysis_003.exe" $dbName
}

if (-not $dbname.Contains(",")) {
	clearExistingData($dbName);
	fetchData($dbName);
	runCodeAnalysis($dbName);
} else {
	$dbs = $dbName.split(",")
	foreach($db in $dbs) {
		clearExistingData($db);
		fetchData($db);
		runCodeAnalysis($db);
	}
}

Write-Host "`nDone!" -foregroundcolor Yellow
