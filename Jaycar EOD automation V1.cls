VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Sheet1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = True
'Global variables
'Workbooks.Open (ThisWorkbook.Path & "\Croydon Blank Master Monthly Stats V283") 'change to monthly doc
'Set EOD = ActiveWorkbook


Sub main()
'runs when excel doc is opened.
eraseData   'clear past weeks data if monday

updateData
Send_mail   'append file to email

ActiveWorkbook.Save
'ActiveWorkbook.Close
End Sub

Sub eraseData()

    If Weekday(Date, vbMonday) = 1 Then
        Worksheets("Sheet1").Range("C4:D10").Value = ""  'Replace value with empty
        Worksheets("Sheet1").Range("G4:K10").Value = ""
    End If
    
End Sub

'First change to buget panel, grab information then go to monthly stat panel and grab information
'then paste information in EOD document
Sub updateData()    'grab budget information and friedman
    Dim EOD As Workbook
    Dim Monthly As Workbook
    
    Dim dayOffset As Integer
    Dim Budget As String, Total As String, LPT As Double, AVG As Double, Trans As Integer
    Dim offset As Integer
    
    offset = 0
    
    dayOffset = Weekday(Date, vbMonday)   ' 1 = monday
    
    Set EOD = ActiveWorkbook
    
    Workbooks.Open (ThisWorkbook.Path & "\Croydon Blank Master Monthly Stats V283") 'change to monthly doc
    Set Monthly = ActiveWorkbook
    
    Budget = Monthly.Worksheets("Daily Sales Budgets").Range("L26").Value   'Store Budget goal
    
    Total = Monthly.Worksheets("Daily & Weekly Totals").Range("G5").Value
    AVG = Monthly.Worksheets("Daily & Weekly Totals").Range("O5").Value
    LPT = Monthly.Worksheets("Daily & Weekly Totals").Range("J5").Value
    Trans = Monthly.Worksheets("Daily & Weekly Totals").Range("I5").Value

    'Start at 3 as Monday counts as 1, and values start at 4 therefore 3+1 = 4
    Cells(3 + dayOffset + offset, 3).Value = Total
    Cells(3 + dayOffset + offset, 8).Value = AVG
    Cells(3 + dayOffset + offset, 9).Value = LPT
    Cells(3 + dayOffset + offset, 7).Value = Trans
    
    EOD.Save

End Sub
Sub Send_mail()
    Dim OutApp As Object
    Dim OutMail As Object
    Set OutApp = CreateObject("Outlook.Application")
    Set OutMail = OutApp.CreateItem(0)
    
    Workbooks.Open Filename:="F:\Content creation\Programming\Jaycar EOD Excel automation\Area 2 Daily EOD reporting IMPROVED.xlsm"
    Set EOD = ActiveWorkbook
    
    'DUPLICATE WORKBOOK OPENING
    Workbooks.Open Filename:="F:\Content creation\Programming\Jaycar EOD Excel automation\Croydon Blank Master Monthly Stats V283" 'change to monthly doc
    Set Monthly = ActiveWorkbook
    
    With OutMail
        .to = Replace(Monthly.Worksheets("Store Details").Range("D11").Value, " ", "") & "@jaycar.com.au"
        .Subject = "EOD report " & Monthly.Worksheets("Store Details").Range("D2").Value & " " & Date
        .cc = ""
        .Body = ""
        
        .Attachments.Add EOD.FullName
        .Display
    End With
    Set OutMail = Nothing
    Set OutApp = Nothing
End Sub
' separate excel for area manager/cc

Private Sub CommandButton1_Click()
    main
End Sub
