import {ConfigProvider} from 'antd';
import UA from "antd/locale/uk_UA"
import './App.css'
import { StyleProvider } from '@ant-design/cssinjs';
import {AuthContextProvider} from "./context/authContext.tsx";
import {MainLayout} from "./components/MainLayout/MainLayout.tsx";


export const Program =()=> {

  return (
  <StyleProvider  hashPriority="high">
      <ConfigProvider locale={UA}>
            <AuthContextProvider>
                <MainLayout/>
            </AuthContextProvider>
      </ConfigProvider>
  </StyleProvider>
  )
}

