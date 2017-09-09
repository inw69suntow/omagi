<%
    string sql = "";
    System.Data.OleDb.OleDbConnection Conn = new System.Data.OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString);
    System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand();
    Conn.Open();
    command.Connection = Conn;
    System.Data.OleDb.OleDbDataReader rs;
 %>

 <div class="menu-container">
    <div class="menu">
     <% if (Session["uGroup"]!=null && ( Session["uGroup"].ToString() == "A" || Session["uGroup"].ToString() == "U" || Session["uGroup"].ToString() == "C" || Session["uGroup"].ToString() == "M"))
        {%>
         <ul>
            <% if (Session["uGroup"].ToString() == "A" || Session["uGroup"].ToString() == "U")
               { %>
                <li><a href="#">บันทึกข้อมูล</a>
                    <ul>
                        <li><a href="#"></a>
                            <ul>
                                <li><a href='<%=Page.ResolveClientUrl("~/mass_entry.aspx")%>'>บันทึกข้อมูลโครงการ</a></li>
								<!--<li><a href='<%=Page.ResolveClientUrl("~/sub_mass_entry.aspx")%>'>บันทึกข้อมูลโครงการย่อย</a></li>--!>
                                <li><a href='<%=Page.ResolveClientUrl("~/train_member_entry.aspx")%>'>บันทึกข้อมูลการฝึกอบรม</a></li>
                                <li><a href='<%=Page.ResolveClientUrl("~/train_course_entry.aspx")%>'>บันทึกข้อมูลหลักสูตรการฝึกอบรม</a></li>
                                <li><a href='<%=Page.ResolveClientUrl("~/member_entry.aspx")%>'>บันทึกข้อมูลสมาชิก</a></li>
                            </ul>
                        </li>
                    </ul>
                </li>
            <%} %>

                 <li><a href="#">รายงาน</a>
                    <ul>
                        <li><a href="#"></a>
                            <ul>
                                <li><a href='<%=Page.ResolveClientUrl("~/report_REP.aspx")%>'>รายงานข้อมูลโครงการและกราฟ</a></li>
                            </ul>
                        </li>
                        <li><a href="#"></a>
                             <ul>
                                
                                        <li><a href="#">รายงานรายละเอียดข้อมูลโครงการ</a>  
                                            <ul>
                                                  <li><a href="#">จำแนกจังหวัด</a>  
                                                      <ul>
                                                         <li><a href='<%=Page.ResolveClientUrl("~/detail_REP.aspx")%>'>ทุกโครงการ</a></li>
                                                         <li><a href='<%=Page.ResolveClientUrl("~/detail_REP.aspx?t=1")%>'>โครงการ กอ.รมน.</a>
                                                          
                                                                        <ul>
                                                                         <%
               sql = "SELECT distinct fl_group_id,fl_group_name";
               sql += " from tb_maingroup where fl_group_status='1' ";
               sql += " and fl_group_type='1' ";
               sql += " order by fl_group_id ";

               command.CommandText = sql;
               rs = command.ExecuteReader();
               while (rs.Read())
               {
                                                                                 %>
                                                                                   <li><a href='<%=Page.ResolveClientUrl("~/detail_REP.aspx?t=1&id="+rs.GetString(0))%>'><%=rs.GetString(1)%></a></li>
                                                                                 <%
               }
               rs.Close();
                                                                          %>
                                                                        
                                                                        </ul>
                                                                     
                                                                  </li>
                                                                   <li><a href='<%=Page.ResolveClientUrl("~/detail_REP.aspx?t=2")%>'>โครงการภาครัฐ</a></li>
                                                                   <li><a href='<%=Page.ResolveClientUrl("~/detail_REP.aspx?t=3")%>'>โครงการภาคประชาชน</a></li>
                                                            </ul>
                                                         </li>
                                                        
                                                         <li><a href="#">จำแนกกลุ่ม</a> 
                                                            <ul>
                                                                <li><a href='<%=Page.ResolveClientUrl("~/detail_REP2.aspx")%>'>ทุกจังหวัด</a> 
                                                                    <ul>
                                                                        <%
                                                                           
               sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
               sql += " and fl_dept_id like '%00' ";
               sql += " order by fl_dept_id ";

               command.CommandText = sql;
               rs = command.ExecuteReader();
               while (rs.Read())
               {
                                                                                %>
                                                                                 <li><a href='<%=Page.ResolveClientUrl("~/detail_REP2.aspx?id="+rs.GetString(0))%>'><%= rs.GetString(1)%></a></li>
                                                                                <%
               }
               rs.Close();
                                                                            %>
                                                                    </ul>
                                                                </li>
                                                                
                                                            </ul>
                                                         </li>

                                                      </ul>
                                         </li>
                                
                            </ul>
                        </li>   
            
                        <li><a href="#"></a>
                            <ul>
                                 <li><a href="#">รายงานสรุปยอดโครงการ</a>                     
                                            <ul>
                                                <li><a href='<%=Page.ResolveClientUrl("~/summary_REP.aspx?t=0")%>'>สรุปภาพรวม</a></li>
                                                <li><a href='<%=Page.ResolveClientUrl("~/summary_REP.aspx?t=1")%>'>รายจังหวัด</a></li>
                                                <%
               sql = "SELECT distinct fl_dept_id,fl_dept_name from tb_dept where fl_dept_status='1' ";
               sql += " and fl_dept_id like '%00' ";
               sql += " order by fl_dept_id ";

               command.CommandText = sql;
               rs = command.ExecuteReader();
               while (rs.Read())
               {  %>
                                                         <li><a href='<%=Page.ResolveClientUrl("~/summary_REP.aspx?t=1&id="+ rs.GetString(0))%>'><%=rs.GetString(1)%></a></li>
                                                        <%
               }
               rs.Close();
                                                %>
                                            </ul>
                                  </li>
                            </ul>
                        </li>


                        <li><a href="#"></a>
                            <ul>
                                <li><a href='<%=Page.ResolveClientUrl("~/man_REP1.aspx")%>'>รายงานข้อมูลบุคคล</a></li>
                            </ul>
                        </li> 
                     </ul>
                 </li>

                  <% if (Session["uGroup"].ToString() == "A" || Session["uGroup"].ToString() == "U")
                     { %>
                 <li><a href='<%=Page.ResolveClientUrl("~/mail_entry.aspx")%>'>ส่งเมล์</a> </li>
                 <%} %>

                 <li><a href='<%=Page.ResolveClientUrl("~/pass_change_entry.aspx")%>'>เปลี่ยนรหัสผ่าน</a> </li>

                 <% if (Session["uGroup"].ToString() == "A")
                    { %>
                 <li><a href='#'>บริหารระบบ</a> 
                    <ul>
                        <li><a href='#'></a> 
                            <ul>
                                  <li><a href='<%=Page.ResolveClientUrl("~/user_entry.aspx")%>'>บันทึกข้อมูลผู้ใช้งาน</a> </li>
                                 <li><a href='<%=Page.ResolveClientUrl("~/unlock_entry.aspx")%>'>ปลดล๊อคผู้ใช้งาน</a> </li>
                                 <li><a href='<%=Page.ResolveClientUrl("~/audit_trail_rep.aspx")%>'>ปูมการใช้ระบบงาน</a> </li>
                            </ul>
                        </li>
                    </ul>
                 </li>
                 <%} %>

            </ul>
       <%} %>
    </div>
</div>
            <%   Conn.Close(); %>
             <script type="text/javascript" src="<%=Page.ResolveClientUrl("~/resources/megamenu-js-master/js/megamenu.js")%>"></script>