const currencyOptions = {
  style: "currency",
  currency: "UAH",
  currencyDisplay: "symbol",
};
export const currencyFormat = (currency: number): string => {
  const formater = new Intl.NumberFormat("uk-UA", currencyOptions);

  return formater.format(currency);
};
