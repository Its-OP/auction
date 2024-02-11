import { createContext, FC, ReactNode, useContext } from "react";
import { authReqType, useAuth } from "../hooks/api/useAuth.ts";

export type authContextType = {
  isAuth: boolean;
  userName: string;
  loading: boolean;
  signIn: (body: authReqType) => Promise<void>;
  signUp: (body: authReqType) => Promise<void>;
};
const defValue = {
  isAuth: false,
  userName: "",
  loading: false,
  signIn: async () => {},
  signUp: async () => {},
};

const AuthContext = createContext<authContextType>(defValue);
export const AuthContextProvider: FC<{ children: ReactNode }> = ({
  children,
}) => {
  const auth = useAuth();

  return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};
export const useAuthContext = (): authContextType => useContext(AuthContext);
