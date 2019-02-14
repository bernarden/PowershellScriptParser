import axios from "axios";
import { GraphData } from "react-force-graph";

export const getGraphData = (): Promise<GraphData> => {
  return axios
    .get("/api/data")
    .then(response => {
      if (response.status === 200) {
        return response.data;
      }
    })
    .catch(error => {
      // tslint:disable-next-line:no-console
      console.log("Server Error", error);
    });
};
