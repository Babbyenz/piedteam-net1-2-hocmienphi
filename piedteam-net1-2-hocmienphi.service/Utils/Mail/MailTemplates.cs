using System.Net;

namespace piedteam_net1_2_hocmienphi.service.Utils.Mail;

public class MailTemplates
{
    public static string GetHelloMailTemplate(string fullName)
    {
        string safeFullName = WebUtility.HtmlEncode(
            string.IsNullOrWhiteSpace(fullName)
                ? "Quý khách"
                : fullName.Trim()
        );

        return $@"
<!DOCTYPE html>
<html lang=""vi"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Chào mừng đến với hệ thống</title>
</head>

<body style=""margin: 0; padding: 0; background-color: #f4f6f8;
             font-family: Arial, Helvetica, sans-serif;"">

    <table role=""presentation""
           width=""100%""
           cellspacing=""0""
           cellpadding=""0""
           border=""0""
           style=""background-color: #f4f6f8; padding: 40px 16px;"">

        <tr>
            <td align=""center"">

                <table role=""presentation""
                       width=""100%""
                       cellspacing=""0""
                       cellpadding=""0""
                       border=""0""
                       style=""max-width: 600px;
                              background-color: #ffffff;
                              border-radius: 12px;
                              overflow: hidden;
                              box-shadow: 0 4px 16px rgba(0, 0, 0, 0.08);"">

                    <tr>
                        <td align=""center""
                            style=""background-color: #2563eb;
                                   padding: 36px 24px;"">

                            <h1 style=""margin: 0;
                                       color: #ffffff;
                                       font-size: 28px;
                                       line-height: 1.4;"">
                                Chào mừng bạn!
                            </h1>

                            <p style=""margin: 10px 0 0;
                                      color: #dbeafe;
                                      font-size: 16px;"">
                                Tài khoản của bạn đã được tạo thành công
                            </p>
                        </td>
                    </tr>

                    <tr>
                        <td style=""padding: 40px 36px;"">

                            <h2 style=""margin: 0 0 20px;
                                       color: #111827;
                                       font-size: 22px;"">
                                Xin chào {safeFullName},
                            </h2>

                            <p style=""margin: 0 0 16px;
                                      color: #4b5563;
                                      font-size: 16px;
                                      line-height: 1.7;"">
                                Chào mừng bạn đến với hệ thống học miễn phí.
                                Tài khoản của bạn hiện đã sẵn sàng để sử dụng.
                            </p>

                            <p style=""margin: 0 0 16px;
                                      color: #4b5563;
                                      font-size: 16px;
                                      line-height: 1.7;"">
                                Bạn có thể đăng nhập và bắt đầu khám phá các khóa học
                                cũng như những tính năng được cung cấp trên hệ thống.
                            </p>

                            <div style=""margin-top: 28px;
                                        padding: 18px;
                                        background-color: #eff6ff;
                                        border-radius: 8px;
                                        border-left: 4px solid #2563eb;"">

                                <p style=""margin: 0;
                                          color: #1e3a8a;
                                          font-size: 14px;
                                          line-height: 1.6;"">
                                    Nếu bạn không thực hiện đăng ký tài khoản này,
                                    vui lòng liên hệ với bộ phận hỗ trợ.
                                </p>
                            </div>

                            <p style=""margin: 30px 0 0;
                                      color: #4b5563;
                                      font-size: 16px;
                                      line-height: 1.7;"">
                                Trân trọng,<br>
                                <strong style=""color: #111827;"">
                                    Đội ngũ Học Miễn Phí
                                </strong>
                            </p>
                        </td>
                    </tr>

                    <tr>
                        <td align=""center""
                            style=""padding: 24px;
                                   background-color: #f9fafb;
                                   border-top: 1px solid #e5e7eb;"">

                            <p style=""margin: 0;
                                      color: #9ca3af;
                                      font-size: 12px;
                                      line-height: 1.6;"">
                                Đây là email được gửi tự động.
                                Vui lòng không trả lời email này.
                            </p>

                            <p style=""margin: 8px 0 0;
                                      color: #9ca3af;
                                      font-size: 12px;"">
                                © {DateTime.Now.Year} Học Miễn Phí.
                                All rights reserved.
                            </p>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>

</body>
</html>";
    }
}