const baseApiURL = "http://localhost:5000/api/";

export const api = {
  auctions: `${baseApiURL}auctions/`,
  auctionsforSearch: `${baseApiURL}auctions`,
  signInUrl: `${baseApiURL}users/signIn`,
  signUpUrl: `${baseApiURL}users/signUp`,

  imageUrl: `${baseApiURL}media/`,

  bids: `${baseApiURL}bids/`,
};
