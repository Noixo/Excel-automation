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
'Public Monthly As Workbooks.Open (ThisWorkbook.Path & blankMonthlyName)
'Public blankMonthlyName As String = "\Croydon Blank Master Monthly Stats V283"
'public MonthlyName As String =  & " Master Monthly Stats " & Format(Date, "m") & " " & Format(Date, "mmmm")

Sub main()
'runs when excel doc is opened.
eraseData   'clear past weeks data if monday

updateData
Send_mail   'append file to email

ActiveWorkbook.Save
'ActiveWorkbook.Close
End Sub

' End of month procedure.
' if end of month and year
' grab data as normal THEN
' save it, close it and open the blank template
' change month and year then save as month
Sub endOfMonth()

    Workbooks.Open (ThisWorkbook.Path & "\Croydon Blank Master Monthly Stats V283")
    Set Monthly = ActiveWorkbook

    If Monthly.Worksheets("Store Details").Range("D5").Value <> Month(Now) Then 'If end of month change month in Monthly document
        Monthly.Worksheets("Store Details").Range("D5").Value = Month(Now)
    End If
    
    'If Monthly.Worksheets("Store Details").Range("D6").Value <> Year(Now) Then
     '   Monthly.Worksheets("Store Details").Range("D6").Value = Year(Now)
    'End If
    
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
    ' Set constants for data offsets
    Const yPosMonthlyTotals As Integer = 4
    Const monthlyBudgetPos As String = "L26"
    Const yPosEODTotals As Integer = 3
    
    'Define workbooks accessed
    Dim EOD As Workbook
    Dim Monthly As Workbook
    
    Dim dayOffset As Integer, offset As Integer
    Dim Budget As String, Total As String, LPT As Double, AVG As Double, Trans As Integer
    
    Dim dd As Date
    dd = "14/01/2020"    '3 = 1st of jan
    Dim aa As Integer
    aa = Application.WorksheetFunction.RoundDown((Day(Now) / 8), 0) * 3
    
    'WHAT DO WE KNOW?
    'we know the month we are in, therefore we know the friedman MAY contain the last monday of the previous month. THEREFORE double verification needed. 1 to find last week of previous month AND check to see whether that week crosses the 1st of current month
   ' eomonth(date(month),0)+1 - Weekday(eomonth(date(month),0) +1- dow)
    
    offset = Day(Now) + aa ' every 7 days add 4
    'If Day(Now) / 7 = 0 Then
    'offset
    'MsgBox (Day(dd))
   ' MsgBox DateValue(Day(Now))
    
    dayOffset = Weekday(Date, vbMonday)   ' 1 = monday
    
    Set EOD = ActiveWorkbook
    
    Workbooks.Open (ThisWorkbook.Path & "\Croydon Blank Master Monthly Stats V283") 'change to monthly doc
    Set Monthly = ActiveWorkbook
    
    Budget = Monthly.Worksheets("Daily Sales Budgets").Range(monthlyBudgetPos).Value   'Store Budget goal
    
    'Grab data from Monthly excel sheet
    Total = Monthly.Worksheets("Daily & Weekly Totals").Cells(yPosMonthlyTotals + offset, 7).Value    '+ offset '+ dayOffset
    
    On Error Resume Next
    AVG = Monthly.Worksheets("Daily & Weekly Totals").Cells(yPosMonthlyTotals + offset, 15).Value    ' AVG MUST BE POPULATED FIRST IN ORDER FOR IT TO WORK
    
    LPT = Monthly.Worksheets("Daily & Weekly Totals").Cells(yPosMonthlyTotals + offset, 10).Value
    Trans = Monthly.Worksheets("Daily & Weekly Totals").Cells(yPosMonthlyTotals + offset, 9).Value

    'Start at 3 as Monday counts as 1, and values start at 4 therefore 3+1 = 4
    Cells(yPosEODTotals + dayOffset, 3).Value = Total
    Cells(yPosEODTotals + dayOffset, 8).Value = AVG
    Cells(yPosEODTotals + dayOffset, 9).Value = LPT
    Cells(yPosEODTotals + dayOffset, 7).Value = Trans
    Cells(yPosEODTotals + dayOffset, 4).Value = Budget
    
    EOD.Save

End Sub
Sub Send_mail()
    Dim OutApp As Object
    Dim OutMail As Object
    Set OutApp = CreateObject("Outlook.Application")
    Set OutMail = OutApp.CreateItem(0)
    
    Workbooks.Open (ThisWorkbook.Path & "\Area 2 Daily EOD reporting IMPROVED.xlsm")
    Set EOD = ActiveWorkbook
    
    'DUPLICATE WORKBOOK OPENING
    Workbooks.Open (ThisWorkbook.Path & "\Croydon Blank Master Monthly Stats V283") 'change to monthly doc
    Set Monthly = ActiveWorkbook
    
    With OutMail
        .to = Left(Monthly.Worksheets("Store Details").Range("D11").Value, 1) & Right(Monthly.Worksheets("Store Details").Range("D11").Value, (Len(Monthly.Worksheets("Store Details").Range("D11").Value) - InStr(Monthly.Worksheets("Store Details").Range("D11").Value, " "))) & "@jaycar.com.au"
        .Subject = "EOD report " & Monthly.Worksheets("Store Details").Range("D2").Value & " " & Date
        .cc = ""
        .Body = ""
        
        .Attachments.Add EOD.FullName
        If Application.WorksheetFunction.EoMonth(today, 0) = Date Then  'add monthly document if end of month
            Monthly.Save
            .Attachments.Add Monthly.FullName
        End If
        .Display
    End With
    Set OutMail = Nothing
    Set OutApp = Nothing
End Sub
' separate excel for area manager/cc

Private Sub CommandButton1_Click()
    main
End Sub
