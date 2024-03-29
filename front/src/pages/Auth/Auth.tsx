import { Button, Form, Input, Card, Typography, message } from "antd";
import { Helmet } from "react-helmet";
import { BASE_TITLE } from "../../const/const.ts";
import { useForm } from "antd/es/form/Form";
import { useAuthContext } from "../../context/authContext.tsx";

type FieldType = {
  username?: string;
  password?: string;
};
export const Auth = () => {
  const { signIn, signUp } = useAuthContext();

  const onFinishFailed = (errorInfo: any) => {
    console.log("Failed:", errorInfo);
    errorInfo.errorFields?.forEach((err: any) => {
      err.errors?.forEach(message.error);
    });
  };

  const [form] = useForm();
  const reg = async () => {
    await signUp(form.getFieldsValue());
  };

  const logIn = async () => {
    await signIn(form.getFieldsValue());
  };

  return (
    <>
      <Helmet>{BASE_TITLE} | Авторизація</Helmet>
      <div
        style={{
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          height: "100%",
        }}
      >
        <Card>
          <Typography.Title level={2} style={{ textAlign: "center" }}>
            Авторизація
          </Typography.Title>
          <Form
            form={form}
            labelCol={{ span: 8 }}
            wrapperCol={{ span: 24 }}
            style={{ maxWidth: 600 }}
            initialValues={{ remember: true }}
            layout={"vertical"}
            onFinishFailed={onFinishFailed}
            autoComplete="off"
          >
            <Form.Item<FieldType>
              label="Логін"
              name="username"
              rules={[{ required: true, message: "Будь ласка введіть логін!" },
               ]}
            >
              <Input />
            </Form.Item>

            <Form.Item<FieldType>
              label="Пароль"
              name="password"
              rules={[
                { required: true, message: "Будь ласка введіть пароль!" },
                {type:"string", min:6, max:16, message:"Пароль має бути 6-16 символів"}
              ]}
            >
              <Input.Password />
            </Form.Item>

            <Form.Item wrapperCol={{ span: 24 }}>
              <div style={{ width: "100%", display: "flex", gap: 10 }}>
                <Button type="default" style={{ flex: 1 }} onClick={reg}>
                  Реєстрація
                </Button>
                <Button type="primary" style={{ flex: 1 }} onClick={logIn}>
                  Увійти
                </Button>
              </div>
            </Form.Item>
          </Form>
        </Card>
      </div>
    </>
  );
};
