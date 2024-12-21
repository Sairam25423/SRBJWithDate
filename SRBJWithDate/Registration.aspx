<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SRBJWithDate.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!--BootStrp Liks-->
    <title>EMP Details</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <!--Sweet Alerts Links-->
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11.6.0/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11.6.0/dist/sweetalert2.all.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="card mt-5">
                <div class="card card-head" style="background: #ADBBDA">
                    <h2 class="text-center mt-2 mb-3" style="color: #AC8968">Employee Details</h2>
                </div>
                <div class="card card-body" style="background: #EDE8F5">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-2 col-xl-2 col-xxl-2"></div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-4 col-xl-4 col-xxl-4">
                            <label><span style="color: red">*</span> Name:</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" AutoCompleteType="Disabled" autocomlete="off" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="* Name is Required." ForeColor="Red" ControlToValidate="txtName" ValidationGroup="SR"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revName" runat="server" ErrorMessage="* Name must be at least 3 characters long and contain only letters and spaces." ForeColor="Red" ControlToValidate="txtName" ValidationGroup="SR" ValidationExpression="^[a-zA-Z\s]{3,}$"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-4 col-xl-4 col-xxl-4">
                            <label><span style="color: red">*</span> Salary:</label>
                            <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" AutoCompleteType="Disabled" MaxLength="5" autocomlete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSalary" runat="server" ErrorMessage="* Salary is Required." ForeColor="Red" ControlToValidate="txtSalary" ValidationGroup="SR"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revSalary" runat="server" ErrorMessage="* Salary must be a valid number." ForeColor="Red" ControlToValidate="txtSalary" ValidationGroup="SR" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-2 col-xl-2 col-xxl-2"></div>
                    </div>
                    <div class="row mt-0">
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-2 col-xl-2 col-xxl-2"></div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-4 col-xl-4 col-xxl-4">
                            <label><span style="color: red">*</span> DepartMent:</label>
                            <asp:DropDownList ID="DpDepartMent" runat="server" CssClass="dropdown-toggle form-control">
                                <asp:ListItem Value="0">--Select Department--</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ErrorMessage="* Department is Required." ForeColor="Red" InitialValue="0" ControlToValidate="DpDepartMent" ValidationGroup="SR"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-4 col-xl-4 col-xxl-4">
                            <label><span style="color: red">*</span> Age:</label>
                            <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" AutoCompleteType="Disabled" autocomlete="off" TextMode="Date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAge" runat="server" ErrorMessage="* Age is Required." ForeColor="Red" ControlToValidate="txtSalary" ValidationGroup="SR"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-2 col-xl-2 col-xxl-2"></div>
                    </div>
                    <div class="row mt-0">
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-2 col-xl-2 col-xxl-2"></div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-8 col-xl-8 col-xxl-8">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-10 col-lg-10 col-xl-10 col-xxl-10">
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" OnChange="previewImage()" />
                                    <asp:RequiredFieldValidator ID="rfvPhoto" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Please upload a photo" ForeColor="Red" ValidationGroup="SR" />
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-10 col-lg-2 col-xl-2 col-xxl-2">
                                    <img id="imgPreview" src="#" alt="Image Preview" style="display: none; max-width: 100px; border: solid 2px; border-radius: 30px;" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-2 col-xl-2 col-xxl-2"></div>
                    </div>
                    <div class="mt-3 text-center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" ValidationGroup="SR" OnClick="btnSubmit_Click" />
                    </div>
                </div>
            </div>

            <div class="card mt-3">
                <div class="row justify-content-center mt-3">
                    <div class="grid-container table-responsive">
                        <asp:GridView ID="GVEmployees" runat="server" CssClass="table table-striped table-bordered text-center table-hover" AutoGenerateColumns="false"
                            OnRowEditing="GVEmployees_RowEditing" OnRowUpdating="GVEmployees_RowUpdating" OnRowDeleting="GVEmployees_RowDeleting"
                            OnRowDataBound="GVEmployees_RowDataBound" OnRowCancelingEdit="GVEmployees_RowCancelingEdit" OnPageIndexChanging="GVEmployees_PageIndexChanging"
                            DataKeyNames="EmployeeCode" AllowPaging="true" PageSize="10" AllowSorting="true">
                            <PagerStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField HeaderText="Employee Id">
                                    <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpid" runat="server" Text='<%# Bind("Empid") %>' ReadOnly="true" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("Name") %>' ReadOnly="true"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEmpNameEdit" runat="server" Text='<%# Bind("Name") %>' CssClass="form-control" MaxLength="100" AutoCompleteType="Disabled" autocomplete="off" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Salary">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalary" runat="server" Text='<%# Bind("Salary") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEmpSalaryEdit" runat="server" Text='<%# Bind("Salary") %>' CssClass="form-control" MaxLength="5" AutoCompleteType="Disabled" autocomplete="off" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("Department") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="DdlEmpDepartmentEdit" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Age">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAge" runat="server" Text='<%# Bind("Age") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEmpAgeEdit" runat="server" Text='<%# Bind("Age", "{0:yyyy-MM-dd}") %>' CssClass="form-control" TextMode="Date" AutoCompleteType="Disabled" autocomplete="off" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Image">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" CssClass="img-fluid" ImageUrl='<%# ResolveUrl("~/Images/" + Eval("Photo").ToString()) %>' Height="50px" Width="50px" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:FileUpload ID="FileUpload2" runat="server" CssClass="form-control" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%# Bind("EmployeeCode") %>' ReadOnly="true" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpPhoto" runat="server" Text='<%# Bind("Photo") %>' ReadOnly="true" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" HeaderText="Actions"
                                    EditText="<span class='btn btn-primary'>Edit</span>" DeleteText="<span class='btn btn-danger'>Delete</span>"
                                    UpdateText="<span class='btn btn-success'>Update</span>" CancelText="<span class='btn btn-warning'>Cancel</span>"
                                    ItemStyle-Width="16%" HeaderStyle-Width="16%" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        function previewImage() {
            var fileInput = document.getElementById("FileUpload1");
            var imgPreview = document.getElementById("imgPreview");

            if (fileInput.files && fileInput.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    imgPreview.src = e.target.result;
                    imgPreview.style.display = "block";
                }
                reader.readAsDataURL(fileInput.files[0]);
            } else {
                imgPreview.src = "";
                imgPreview.style.display = "none";
            }
        }

        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };
    </script>
</body>
</html>
