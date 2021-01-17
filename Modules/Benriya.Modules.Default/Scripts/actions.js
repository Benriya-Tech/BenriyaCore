//---------------------------------------------------



//---------------------------------------------------
//---------------------------------------------------

const requestData = {
  method : 'getUsers'
};


const usersPromise = fetch('/app', {
  method : 'POST',
  body : JSON.stringify(requestData)
}).then(response => {
  if (!response.ok) {
    throw new Error("Got non-2XX response from API server.");
  }
  return response.json();
}).then(responseData => {
  return responseData.users;
});