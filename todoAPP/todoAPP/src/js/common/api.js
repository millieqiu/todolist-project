import axios from "axios";
import Swal from "sweetalert2";

const instance = axios.create({
  baseURL: "/",
});

// Add a response interceptor
instance.interceptors.response.use(
  function (response) {
    return response;
  },
  function (error) {
    if (error.response) {
      console.log(error.response.data);
      console.log(error.response.status);
      console.log(error.response.headers);
      Swal.fire({
        title: "Error!",
        text: `${error.response.data}`,
        icon: "error",
        confirmButtonText: "OK",
      });
    }
    return Promise.reject(error);
  }
);

export default instance;