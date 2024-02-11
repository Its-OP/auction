import { useState } from "react";
import { useCookies } from "react-cookie";

export enum HTTP_METHOD {
  GET = "GET",
  POST = "POST",
  PUT = "PUT",
}
export const useHttp = () => {
  const [cookies, _, removeCookies] = useCookies(["token"]);

  const [loading, setLoading] = useState<boolean>(false);
  const request = async (
    url: string,
    method: HTTP_METHOD = HTTP_METHOD.GET,
    body?: object
  ) => {
    try {
      setLoading(true);

      const response = await fetch(url, {
        method,
        body: body && JSON.stringify(body),
        headers: {
          "Content-Type": "application/json",
          Authorization: cookies.token ? `Bearer ${cookies.token}` : "",
        },
      });

      if (response.status === 401) {
        removeCookies("token");
      }

      if (response.status === 200) {
        return response.json();
      }
    } catch (err) {
      return { err };
    } finally {
      setLoading(false);
    }
  };

  return { request, loading };
};
