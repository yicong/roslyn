' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis.Editor.UnitTests.Extensions

Namespace Microsoft.VisualStudio.LanguageServices.UnitTests.CodeModel
    Public MustInherit Class AbstractCodeFunctionTests
        Inherits AbstractCodeElementTests(Of EnvDTE80.CodeFunction2)

        Protected Overrides Function GetStartPointFunc(codeElement As EnvDTE80.CodeFunction2) As Func(Of EnvDTE.vsCMPart, EnvDTE.TextPoint)
            Return Function(part) codeElement.GetStartPoint(part)
        End Function

        Protected Overrides Function GetEndPointFunc(codeElement As EnvDTE80.CodeFunction2) As Func(Of EnvDTE.vsCMPart, EnvDTE.TextPoint)
            Return Function(part) codeElement.GetEndPoint(part)
        End Function

        Protected Overrides Function GetAccess(codeElement As EnvDTE80.CodeFunction2) As EnvDTE.vsCMAccess
            Return codeElement.Access
        End Function

        Protected Overrides Function GetComment(codeElement As EnvDTE80.CodeFunction2) As String
            Return codeElement.Comment
        End Function

        Protected Overrides Function GetDocComment(codeElement As EnvDTE80.CodeFunction2) As String
            Return codeElement.DocComment
        End Function

        Protected Overrides Function GetFullName(codeElement As EnvDTE80.CodeFunction2) As String
            Return codeElement.FullName
        End Function

        Protected Function GetFunctionKind(codeElement As EnvDTE80.CodeFunction2) As EnvDTE.vsCMFunction
            Return codeElement.FunctionKind
        End Function

        Protected Overrides Function GetKind(codeElement As EnvDTE80.CodeFunction2) As EnvDTE.vsCMElement
            Return codeElement.Kind
        End Function

        Protected Overrides Function GetMustImplement(codeElement As EnvDTE80.CodeFunction2) As Boolean
            Return codeElement.MustImplement
        End Function

        Protected Overrides Function GetName(codeElement As EnvDTE80.CodeFunction2) As String
            Return codeElement.Name
        End Function

        Protected Overridable Function GetIsOverloaded(codeElement As EnvDTE80.CodeFunction2) As Boolean
            Return codeElement.IsOverloaded
        End Function

        Protected Overridable Function GetOverloads(codeElement As EnvDTE80.CodeFunction2) As EnvDTE.CodeElements
            Return codeElement.Overloads
        End Function

        Protected Overrides Function GetOverrideKind(codeElement As EnvDTE80.CodeFunction2) As EnvDTE80.vsCMOverrideKind
            Return codeElement.OverrideKind
        End Function

        Protected Overrides Function GetParent(codeElement As EnvDTE80.CodeFunction2) As Object
            Return codeElement.Parent
        End Function

        Protected Overrides Function GetPrototype(codeElement As EnvDTE80.CodeFunction2, flags As EnvDTE.vsCMPrototype) As String
            Return codeElement.Prototype(flags)
        End Function

        Protected Overrides Function GetAccessSetter(codeElement As EnvDTE80.CodeFunction2) As Action(Of EnvDTE.vsCMAccess)
            Return Sub(access) codeElement.Access = access
        End Function

        Protected Overrides Function GetIsSharedSetter(codeElement As EnvDTE80.CodeFunction2) As Action(Of Boolean)
            Return Sub(value) codeElement.IsShared = value
        End Function

        Protected Overrides Function GetMustImplementSetter(codeElement As EnvDTE80.CodeFunction2) As Action(Of Boolean)
            Return Sub(value) codeElement.MustImplement = value
        End Function

        Protected Overrides Function GetOverrideKindSetter(codeElement As EnvDTE80.CodeFunction2) As Action(Of EnvDTE80.vsCMOverrideKind)
            Return Sub(value) codeElement.OverrideKind = value
        End Function

        Protected Overrides Function GetNameSetter(codeElement As EnvDTE80.CodeFunction2) As Action(Of String)
            Return Sub(name) codeElement.Name = name
        End Function

        Protected Overrides Function GetTypeProp(codeElement As EnvDTE80.CodeFunction2) As EnvDTE.CodeTypeRef
            Return codeElement.Type
        End Function

        Protected Overrides Function GetTypePropSetter(codeElement As EnvDTE80.CodeFunction2) As Action(Of EnvDTE.CodeTypeRef)
            Return Sub(value) codeElement.Type = value
        End Function

        Protected Overrides Function AddParameter(codeElement As EnvDTE80.CodeFunction2, data As ParameterData) As EnvDTE.CodeParameter
            Return codeElement.AddParameter(data.Name, data.Type, data.Position)
        End Function

        Protected Overrides Sub RemoveChild(codeElement As EnvDTE80.CodeFunction2, child As Object)
            codeElement.RemoveParameter(child)
        End Sub

        Protected Overridable Function ExtensionMethodExtender_GetIsExtension(codeElement As EnvDTE80.CodeFunction2) As Boolean
            Throw New NotSupportedException
        End Function

        Protected Overridable Function PartialMethodExtender_GetIsPartial(codeElement As EnvDTE80.CodeFunction2) As Boolean
            Throw New NotSupportedException
        End Function

        Protected Overridable Function PartialMethodExtender_GetIsDeclaration(codeElement As EnvDTE80.CodeFunction2) As Boolean
            Throw New NotSupportedException
        End Function

        Protected Overridable Function PartialMethodExtender_GetHasOtherPart(codeElement As EnvDTE80.CodeFunction2) As Boolean
            Throw New NotSupportedException
        End Function

        Protected Sub TestCanOverride(code As XElement, expected As Boolean)
            Using state = CreateCodeModelTestState(GetWorkspaceDefinition(code))
                Dim codeElement = state.GetCodeElementAtCursor(Of EnvDTE80.CodeFunction2)()
                Assert.NotNull(codeElement)

                Assert.Equal(expected, codeElement.CanOverride)
            End Using
        End Sub

        Protected Sub TestSetCanOverride(code As XElement, expectedCode As XElement, value As Boolean)
            TestSetCanOverride(code, expectedCode, value, NoThrow(Of Boolean)())
        End Sub

        Protected Sub TestSetCanOverride(code As XElement, expectedCode As XElement, value As Boolean, action As SetterAction(Of Boolean))
            Using state = CreateCodeModelTestState(GetWorkspaceDefinition(code))
                Dim codeElement = state.GetCodeElementAtCursor(Of EnvDTE80.CodeFunction2)()
                Assert.NotNull(codeElement)

                action(value, Sub(v) codeElement.CanOverride = v)

                Dim text = state.GetDocumentAtCursor().GetTextAsync().Result.ToString()

                Assert.Equal(expectedCode.NormalizedValue.Trim(), text.Trim())
            End Using
        End Sub

        Protected Sub TestIsOverloaded(code As XElement, expectedOverloaded As Boolean)
            Using state = CreateCodeModelTestState(GetWorkspaceDefinition(code))
                Dim codeElement = state.GetCodeElementAtCursor(Of EnvDTE80.CodeFunction2)()
                Assert.NotNull(codeElement)

                Dim overloaded = GetIsOverloaded(codeElement)
                Assert.Equal(expectedOverloaded, overloaded)
            End Using
        End Sub

        Protected Sub TestOverloadsUniqueSignatures(code As XElement, ParamArray expectedOverloadNames As String())
            Using state = CreateCodeModelTestState(GetWorkspaceDefinition(code))
                Dim codeElement = state.GetCodeElementAtCursor(Of EnvDTE80.CodeFunction2)()
                Assert.NotNull(codeElement)

                Dim actualOverloads = GetOverloads(codeElement)
                Assert.Equal(expectedOverloadNames.Count, actualOverloads.Count)
                For index = 1 To actualOverloads.Count
                    Dim codeFunction = CType(actualOverloads.Item(index), EnvDTE80.CodeFunction2)
                    Dim signature = GetPrototype(codeFunction, EnvDTE.vsCMPrototype.vsCMPrototypeUniqueSignature)
                    Assert.True(expectedOverloadNames.Contains(signature))
                Next
            End Using
        End Sub

        Protected Sub TestFunctionKind(code As XElement, expected As EnvDTE.vsCMFunction)
            Using state = CreateCodeModelTestState(GetWorkspaceDefinition(code))
                Dim codeElement = state.GetCodeElementAtCursor(Of EnvDTE80.CodeFunction2)()
                Assert.NotNull(codeElement)

                Assert.Equal(expected, codeElement.FunctionKind)
            End Using
        End Sub

        Protected Sub TestExtensionMethodExtender_IsExtension(code As XElement, expected As Boolean)
            Using state = CreateCodeModelTestState(GetWorkspaceDefinition(code))
                Dim codeElement = state.GetCodeElementAtCursor(Of EnvDTE80.CodeFunction2)()
                Assert.NotNull(codeElement)

                Assert.Equal(expected, ExtensionMethodExtender_GetIsExtension(codeElement))
            End Using
        End Sub

        Protected Sub TestPartialMethodExtender_IsPartial(code As XElement, expected As Boolean)
            Using state = CreateCodeModelTestState(GetWorkspaceDefinition(code))
                Dim codeElement = state.GetCodeElementAtCursor(Of EnvDTE80.CodeFunction2)()
                Assert.NotNull(codeElement)

                Assert.Equal(expected, PartialMethodExtender_GetIsPartial(codeElement))
            End Using
        End Sub

        Protected Sub TestPartialMethodExtender_IsDeclaration(code As XElement, expected As Boolean)
            Using state = CreateCodeModelTestState(GetWorkspaceDefinition(code))
                Dim codeElement = state.GetCodeElementAtCursor(Of EnvDTE80.CodeFunction2)()
                Assert.NotNull(codeElement)

                Assert.Equal(expected, PartialMethodExtender_GetIsDeclaration(codeElement))
            End Using
        End Sub

        Protected Sub TestPartialMethodExtender_HasOtherPart(code As XElement, expected As Boolean)
            Using state = CreateCodeModelTestState(GetWorkspaceDefinition(code))
                Dim codeElement = state.GetCodeElementAtCursor(Of EnvDTE80.CodeFunction2)()
                Assert.NotNull(codeElement)

                Assert.Equal(expected, PartialMethodExtender_GetHasOtherPart(codeElement))
            End Using
        End Sub

    End Class
End Namespace
