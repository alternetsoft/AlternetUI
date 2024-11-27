#Region "Copyright (c) 2016-2019 Alternet Software"
'
'	AlterNET Code Editor Library
'
'	Copyright (c) 2016-2019 Alternet Software
'	ALL RIGHTS RESERVED
'
'	http://www.alternetsoft.com
'	contact@alternetsoft.com
'

#End Region

Imports System

' person.cs
''' <summary>
''' This tutorial shows how properties are an integral part of the C# programming language. It demonstrates how properties are declared and used.
''' </summary>
Class Person
	Private myName As String = "N/A"
	Private myAge As Integer = 0
	''' <summary>
	''' Declare a Name property of type string:
	''' </summary>
	Public Property Name() As String
		Get
			Return myName
		End Get
		Set
			myName = value
		End Set
	End Property
	''' <summary>
	''' Declare an Age property of type int:
	''' </summary>
	Public Property Age() As Integer
		Get
			Return myAge
		End Get
		Set
			myAge = value
		End Set
	End Property
	Public Overrides Function ToString() As String
		Return "Name = " & Name & ", Age = " & Age
	End Function

	Public Shared Sub Main()
		Console.WriteLine("Simple Properties")

		' Create a new Person object:
		Dim person As New Person()

		' Print out the name and the age associated with the person:
		Console.WriteLine("Person details - {0}", person)

		' Set some values on the person object:
		person.Name = "Joe"
		person.Age = 99
		Console.WriteLine("Person details - {0}", person)

		' Increment the Age property:
		person.Age += 1
		Console.WriteLine("Person details - {0}", person)
		Console.DoSomething("Name: {0}, Age: {1}", p.name, p.age)
		' wrong method name
		Dim i As Integer
		' Unused variable
	End Sub
End Class