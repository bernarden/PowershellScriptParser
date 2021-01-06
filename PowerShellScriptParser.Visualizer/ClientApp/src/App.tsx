import * as React from "react";
import { ForceGraph3D, GraphData } from "react-force-graph";

import { getGraphData } from "./api/dataApi";
import "./App.css";

interface IState {
  data?: GraphData;
}

class App extends React.Component<{}, IState> {
  constructor(props:any) {
      super(props);
      this.state = {};
  }

  public componentWillMount() {
    getGraphData().then(data => this.setState({ data }));
  }

  public render() {
    const data  = this.state.data;
    return (
      <div className="App">
        {data && (
          <ForceGraph3D
            graphData={data}
            nodeLabel="id"
            nodeAutoColorBy="group"
            linkDirectionalParticles="value"
            // tslint:disable-next-line:jsx-no-lambda
            linkDirectionalParticleSpeed={d => d.value * 0.001}
          />
        )}
      </div>
    );
  }
}

export default App;
