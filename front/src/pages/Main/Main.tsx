import { Alert, Button, Col, Empty, Form, Input, Radio, Row, Spin } from "antd";
import { LotCard } from "../../components/LotCard/LotCard.tsx";
import { Helmet } from "react-helmet";
import { BASE_TITLE } from "../../const/const.ts";
import { useLot } from "../../hooks/use/useLot.ts";
import { useEffect } from "react";

export const Main = () => {
  const { lots, getLots, lotApiLoading, loadMore, page } = useLot();

  useEffect(() => {
    getLots();
  }, []);

  const searchLots = async (values: any) => {
    const q = `?search=${values.search ?? ""}&sort=${values.sort ?? ""}`;

    getLots(q);
  };

  const getMoreLots = async () => {
    loadMore();
  };

  return (
    <>
      <Helmet>
        <title>{BASE_TITLE}</title>
      </Helmet>
      <Row>
        <Form
          onFinish={searchLots}
          style={{ display: "flex", width: "100%", gap: 20 }}
        >
          <Form.Item style={{ flex: 1 }} label={"Пошук"} name={"search"}>
            <Input allowClear />
          </Form.Item>
          <Form.Item name={"sort"}>
            <Radio.Group defaultValue="lastCreated">
              <Radio.Button value="lastCreated">Останні</Radio.Button>
              <Radio.Button value="desc">Ціна від більших</Radio.Button>
              <Radio.Button value="asc">Ціна від меньших</Radio.Button>
            </Radio.Group>
          </Form.Item>
          <Form.Item>
            <Button type={"primary"} htmlType={"submit"}>
              Застосувати
            </Button>
          </Form.Item>
        </Form>
      </Row>

      <Row gutter={[30, 30]}>
        {lotApiLoading ? (
          <Spin style={{ margin: "auto", marginTop: "20vh" }} size={"large"}>
            <Alert message={"Завантаження"} type={"info"} />
          </Spin>
        ) : (
          <>
            {lots.map((lot) => (
              <Col key={lot.id} md={12} lg={8}>
                <LotCard lot={lot} />
              </Col>
            ))}
          </>
        )}
        {!lotApiLoading && lots.length === 0 ? <Empty /> : null}
      </Row>
      {lots.length === page * 10 ? (
        <div
          style={{
            marginTop: 30,
            width: "100%",
            display: "flex",
            justifyContent: "center",
          }}
        >
          <Button size={"large"} type={"primary"} onClick={getMoreLots}>
            Завантажитьи ще
          </Button>
        </div>
      ) : null}
    </>
  );
};
