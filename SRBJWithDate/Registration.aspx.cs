using System;
using System.IO;
using System.Web;
using System.Data;
using System.Web.UI;
using SRBJWithDate.App_Start;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace SRBJWithDate
{
    public partial class Registration : System.Web.UI.Page
    {
        DataAccessLayer objDL = new DataAccessLayer();
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridData(); BindDepartments();
                txtAge.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
                txtAge.Attributes["min"] = DateTime.Now.ToString("1947-01-01");
            }
        }
        #endregion
        #region RowEdit
        protected void GVEmployees_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GVEmployees.EditIndex = e.NewEditIndex; BindGridData();
        }
        #endregion
        #region RowUpdate
        protected void GVEmployees_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow Row = GVEmployees.Rows[e.RowIndex];
            string EmpId = (Row.FindControl("lblEmpid") as Label).Text.Trim();
            string EmpCode = (Row.FindControl("lblEmpCode") as Label).Text.Trim();
            string EmpName = (Row.FindControl("txtEmpNameEdit") as TextBox).Text.Trim();
            string EmpSalary = (Row.FindControl("txtEmpSalaryEdit") as TextBox).Text.Trim();
            string EmpAgeText = (Row.FindControl("txtEmpAgeEdit") as TextBox).Text.Trim();
            string EmpDepartment = (Row.FindControl("DdlEmpDepartmentEdit") as DropDownList).SelectedItem.Text.ToString();
            string EmpPhoto = (Row.FindControl("lblEmpPhoto") as Label).Text.Trim();
            FileUpload FileUpload = Row.FindControl("FileUpload2") as FileUpload;
            if (FileUpload.HasFile)
            {
                string PathExtenction = Path.GetExtension(FileUpload.FileName).ToLower();
                if (PathExtenction == ".jpg" || PathExtenction == ".jpeg" || PathExtenction == ".png")
                {
                    string RootFolder = HttpContext.Current.Server.MapPath("~/Images/");
                    string DestinationPath = Path.Combine(RootFolder, EmpCode + PathExtenction);

                    if (File.Exists(DestinationPath))
                    {
                        File.Delete(DestinationPath);
                    }
                    FileUpload.SaveAs(DestinationPath);
                }
            }
            else { };

            if (!(string.IsNullOrEmpty(EmpName.Trim()) && string.IsNullOrEmpty(EmpSalary.Trim()) && string.IsNullOrEmpty(EmpAgeText.Trim()) && string.IsNullOrEmpty(EmpDepartment.Trim())))
            {
                bool ValidFields = ValidationCheck(EmpName, EmpSalary, EmpAgeText);
                if (ValidFields == true)
                {
                    DateTime Dt1 = Convert.ToDateTime(EmpAgeText);
                    DateTime Dt2 = DateTime.Now;
                    int TAge = Dt2.Year - Dt1.Year;
                    if (TAge >= 20)
                    {
                        string SP = "Sp_UpdateDetails"; string[] ParameterNames = { "@EmpId", "@Name", "@Salary", "@Department", "@Age", "@Photo" };
                        string[] ParameterValues = { EmpId, EmpName, EmpSalary, EmpDepartment, EmpAgeText.ToString(), $"{EmpCode}.jpg" };
                        bool Result = objDL.InsertData(SP, ParameterNames, ParameterValues);
                        if (Result == true)
                        {
                            GVEmployees.EditIndex = -1; BindGridData();
                        }
                    }
                    else
                    {
                        string script1 = @"
                            Swal.fire({
                            title: 'Failure!',
                            text: 'Age should grater than 18.',
                            icon: 'error',
                            confirmButtonText: 'OK'
                            });";
                        ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script1, true);
                    }
                }
                else
                {
                    string script1 = @"
                     Swal.fire({
                     title: 'Failure!',
                     text: 'Validations failed.',
                     icon: 'error',
                     confirmButtonText: 'OK'
                     });";
                    ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script1, true);
                }
            }
            else { }
        }
        #endregion
        #region RowDelete
        protected void GVEmployees_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string EmployeeCode = GVEmployees.DataKeys[e.RowIndex].Value.ToString();
            string SP = "Sp_DeleteDetails"; string[] ParameterNames = { "@EmployeeCode" };
            string[] ParameterValues = { EmployeeCode };
            bool Result = objDL.InsertData(SP, ParameterNames, ParameterValues);
            if (Result == true)
            {
                BindGridData();
                string script = @"
                Swal.fire({
                title: 'Success!',
                text: 'User deleted successfully.',
                icon: 'success',
                confirmButtonText: 'OK'
                });";
                ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script, true);
            }
            else
            {
                string script = @"
                Swal.fire({
                title: 'Failure!',
                text: 'Something went wrong.',
                icon: 'error',
                confirmButtonText: 'OK'
                });";
                ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script, true);
            }
        }
        #endregion
        #region RowDataBound
        protected void GVEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtEmpAgeEdit = (TextBox)e.Row.FindControl("txtEmpAgeEdit");
                if (txtEmpAgeEdit != null)
                {
                    DateTime AgeDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "Age"));
                    txtEmpAgeEdit.Text = AgeDate.ToString("yyyy-MM-dd");
                }
                DropDownList DpDep = e.Row.FindControl("DdlEmpDepartmentEdit") as DropDownList;
                if (DpDep != null)
                {
                    string SP = "Sp_LoadDepartments"; string[] ParameterNames = null; string[] ParameterValues = null;
                    DataSet Ds = objDL.RetrivedData(SP, ParameterNames, ParameterValues);
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        DpDep.DataSource = Ds.Tables[0];
                        DpDep.DataTextField = "DepartmentName";
                        DpDep.DataValueField = "DepartmentId";
                        DpDep.Items.Insert(0, new ListItem("--Select--", "0"));
                        DpDep.DataBind();

                        string CurrentDepartment = DataBinder.Eval(e.Row.DataItem, "Department").ToString();
                        DpDep.SelectedValue = CurrentDepartment;
                    }
                }
            }
        }
        #endregion
        #region RowCancel
        protected void GVEmployees_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GVEmployees.EditIndex = -1; BindGridData();
        }
        #endregion
        #region Pagination
        protected void GVEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVEmployees.PageIndex = e.NewPageIndex; BindGridData();
        }
        #endregion
        #region BtnSubmit
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(txtName.Text.Trim()) && string.IsNullOrWhiteSpace(txtSalary.Text.Trim()) && string.IsNullOrEmpty(txtAge.Text.Trim())) && DpDepartMent.SelectedIndex > 0)
            {
                DateTime Date = Convert.ToDateTime(txtAge.Text.Trim());
                string formattedDate = Date.ToString("dd-MM-yyyy"); DateTime Dt1 = Convert.ToDateTime(formattedDate);
                DateTime Dt2 = DateTime.Now;
                int TAge = Dt2.Year - Dt1.Year;
                if (TAge >= 20)
                {
                    string SP = "Sp_InsertData"; string[] ParameterNames = { "@Name", "@Salary", "@Age", "@Department" };
                    string[] ParameterValues = { txtName.Text.Trim(), txtSalary.Text.Trim(), txtAge.Text.Trim(), DpDepartMent.SelectedItem.Text };
                    string EmpCode = objDL.InsertExecuteScalar(SP, ParameterNames, ParameterValues);
                    if (!string.IsNullOrEmpty(EmpCode))
                    {
                        if (FileUpload1.HasFile)
                        {
                            string PathExtenction = Path.GetExtension(FileUpload1.FileName).ToLower();
                            if (PathExtenction == ".jpg" || PathExtenction == ".jpeg" || PathExtenction == ".png")
                            {
                                string RootFolder = HttpContext.Current.Server.MapPath("~/Images/");
                                string DestinationPath = Path.Combine(RootFolder, EmpCode + PathExtenction);
                                FileUpload1.SaveAs(DestinationPath);
                                string SP1 = "Sp_UpdateImage"; string[] PMNames = { "@EmpCode", "@Photo" };
                                string[] PMValues = { EmpCode, $"{EmpCode}.jpg" };
                                bool Result = objDL.InsertData(SP1, PMNames, PMValues);
                                if (Result == true)
                                {
                                    string script = @"
                                    Swal.fire({
                                    title: 'Success!',
                                    text: 'Details inserted successfully.',
                                    icon: 'success',
                                    confirmButtonText: 'OK'
                                    });";
                                    ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script, true);
                                    txtName.Text = txtSalary.Text = txtAge.Text = txtSalary.Text = string.Empty; DpDepartMent.SelectedIndex = -1; BindGridData();
                                }
                                else
                                {
                                    string script1 = @"
                                    Swal.fire({
                                    title: 'Failure!',
                                    text: 'Something went wrong.',
                                    icon: 'error',
                                    confirmButtonText: 'OK'
                                    });";
                                    ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script1, true);
                                }
                            }
                            else
                            {
                                string script1 = @"
                                Swal.fire({
                                title: 'Failure!',
                                text: 'File upload accept Jpg,Jpeg,Png Only.',
                                icon: 'error',
                                confirmButtonText: 'OK'
                                });";
                                ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script1, true);
                            }
                        }
                        else
                        {
                            string script2 = @"
                            Swal.fire({
                            title: 'Failure!',
                            text: 'Please upload Image.',
                            icon: 'error',
                            confirmButtonText: 'OK'
                            });";
                            ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script2, true);
                        }
                    }
                    else
                    {
                        string script = @"
                        Swal.fire({
                        title: 'Failure!',
                        text: 'Something went wrong.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                        });";
                        ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script, true);
                    }
                }
                else
                {
                    string script = @"
                        Swal.fire({
                        title: 'Failure!',
                        text: 'Age should be greater than 18.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                        });";
                    ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script, true);
                }
            }
        }
        #endregion
        #region BindData
        public void BindGridData()
        {
            GVEmployees.DataSource = null;
            GVEmployees.DataBind();

            string SP = "Sp_GetData"; string[] ParameterNames = null; string[] ParameterValues = null;
            DataSet Ds = objDL.RetrivedData(SP, ParameterNames, ParameterValues);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                GVEmployees.DataSource = Ds.Tables[0];
                GVEmployees.DataBind();
                GVEmployees.Visible = true;
            }
        }
        #endregion
        #region BindDropDown
        public void BindDepartments()
        {
            string SP = "Sp_LoadDepartments"; string[] ParameterNames = null; string[] ParameterValues = null;
            DataSet Ds = objDL.RetrivedData(SP, ParameterNames, ParameterValues);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DpDepartMent.DataSource = Ds.Tables[0];
                DpDepartMent.DataTextField = "DepartmentName";
                DpDepartMent.DataValueField = "DepartmentId";
                DpDepartMent.DataBind();
                DpDepartMent.Items.Insert(0, new ListItem("---Select Department---", "0"));

            }
        }
        #endregion
        #region Validations Check
        public bool ValidationCheck(string EmpName, string EmpSalary, string EmpAge)
        {
            string NamePattern = @"^[a-zA-Z\s]{3,}$";
            string SalaryPattern = @"^\d+$";
            //string AgePattern = @"^(?:[2-9][0-9]{1,2}|[1-9][0-9]{3,})$";
            bool Validation = true;
            string validationMessage = string.Empty;
            if (!Regex.IsMatch(EmpName, NamePattern))
            {
                Validation = false; validationMessage = "Invalid Name format.";
            }
            else if (!Regex.IsMatch(EmpSalary, SalaryPattern))
            {
                Validation = false; validationMessage = "Invalid Salary format.";
            }
            //else if (!Regex.IsMatch(EmpAge, AgePattern))
            //{
            //    Validation = false; validationMessage = "Invalid Age format.";
            //}
            if (!Validation)
            {
                string script = $@"
                Swal.fire({{
                    title: 'Validation Error',
                    text: '{validationMessage}',
                    icon: 'error',
                    confirmButtonText: 'OK'
                }});";
                ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script, true);
            }
            return Validation;
        }
        #endregion
    }
}