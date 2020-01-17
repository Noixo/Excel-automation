VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Sheet1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = True
Sub Auto_Open()
'runs when excel doc is opened.
eraseData   'clear past weeks data if monday

Send_mail   'append file to email

ActiveWorkbook.Save
'ActiveWorkbook.Close
End Sub

Sub eraseData()

    If Weekday(Date, vbMonday) = 1 Then
    Worksheets("Sheet1").Range("A1:C10").Clear  'change with the actual range
    End If
    
End Sub

'First change to buget panel, grab information then go to monthly stat panel and grab information
'then paste information in EOD document
Sub updateData()    'grab budget information and friedman
    Dim EOD As Workbook
    Dim Monthly As Workbook
    
    Dim dayOffset As Integer
    Dim Budget As Double, Total As Double, LPT As Double, AVG As Double
    
    dayOffset = Weekday(Date, vbMonday)   ' 1 = monday
    
    Set EOD = ActiveWorkbook
    
    Workbooks.Open Filename:="C:\\Book.xls" 'change to monthly doc
    Set Monthly = ActiveWorkbook
    Worksheets("Sheet1").Select ' change to budget panel
    
    Budget = Range("A1")
    
    Worksheets("Sheet2").Select ' change to monthly stats
    
    
    'grab total. LPT and AVG data
    
    ActiveWorkbook.Close
    EOD.Activate 'change back to EOD doc
    
    'bad code
    
    ActiveCell.Offset(1, (1 + dayOffset)).Select
    ActiveCell.Value = Budget
    
    ActiveCell.Offset(1, (1 + dayOffset)).Select
    ActiveCell.Value = Total
    
    ActiveCell.Offset(1, (1 + dayOffset)).Select
    ActiveCell.Value = LPT
    
    ActiveCell.Offset(1, (1 + dayOffset)).Select
    ActiveCell.Value = AVG
    
    'paste information in correct dates
    
    EOD.Save

End Sub
Sub Send_mail()
    
    Dim OutApp As Object
    Dim OutMail As Object
    Set OutApp = CreateObject("Outlook.Application")
    Set OutMail = OutApp.CreateItem(0)
    With OutMail
        '.to = "test@test.com"
        .Subject = "EOD report " & Date
        .cc = ""
        '.Body = ""
        .Attachments.Add ActiveWorkbook.FullName
        .Display
    End With
    Set OutMail = Nothing
    Set OutApp = Nothing
End Sub
' separate excel for area manager/cc
