using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;
using System.IO;

namespace AutogenerateCode
{
    class AutoGenerateService
    {
        static void Main(string[] args)
        {
            var path = "C:\\Users\\rhinojosa\\Desktop\\Microservice.cs";

            // Create a compile unit
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            // Define a Namespace
            CodeNamespace apiNamespace = new CodeNamespace("Microservice.Controllers");
            // Import Namespaces
            apiNamespace.Imports.Add(new CodeNamespaceImport("System"));
            apiNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            apiNamespace.Imports.Add(new CodeNamespaceImport("System.Linq"));
            apiNamespace.Imports.Add(new CodeNamespaceImport("System.Threading.Tasks"));
            apiNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.AspNetCore.Mvc"));

            compileUnit.Namespaces.Add(apiNamespace);

            // Define a class
            CodeTypeDeclaration controllerClass = new CodeTypeDeclaration("ValuesController");
            // Inherit a type
            controllerClass.BaseTypes.Add("ControllerBase");
            apiNamespace.Types.Add(controllerClass);

            // Set custom attribute
            CodeAttributeDeclaration routeAttribute = new CodeAttributeDeclaration("Route(\"api/[controller]\")");
            CodeAttributeDeclaration apiAttribute = new CodeAttributeDeclaration("ApiController");
            controllerClass.CustomAttributes.Add(routeAttribute);
            controllerClass.CustomAttributes.Add(apiAttribute);
            // Set class attribute
            controllerClass.IsClass = true;
            controllerClass.TypeAttributes = TypeAttributes.Public;

            // Declare a method
            CodeMemberMethod getMethod = new CodeMemberMethod();
            CodeAttributeDeclaration getAttribute = new CodeAttributeDeclaration("HttpGet");
            CodeCommentStatement getComment = new CodeCommentStatement("GET api/values");
            CodeTypeReference returnType = new CodeTypeReference("ActionResult<IEnumerable<string>>");
            CodeMethodReturnStatement codeExpression  = new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("new string[] { \"value1\", \"value2\" }"));

            getMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            getMethod.CustomAttributes.Add(getAttribute);
            getMethod.Comments.Add(getComment);
            getMethod.Name = "Get";
            getMethod.ReturnType = returnType;
            getMethod.Statements.Add(codeExpression);
            controllerClass.Members.Add(getMethod);


            // Generate C# source code
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(path))
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, sourceWriter, options);
            }
        }
    }
}
