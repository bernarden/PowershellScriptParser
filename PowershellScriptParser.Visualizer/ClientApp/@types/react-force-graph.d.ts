declare module "react-force-graph" {
  import * as React from "react";

  export interface GraphNode {
    id: string;
    group: number;
  }

  export interface GraphLink {
    source: string;
    target: string;
    value: number;
  }

  export interface GraphData {
    nodes: GraphNode[];
    links: GraphLink[];
  }

  export interface ForceGraph3DProps {
    graphData: GraphData;
    nodeLabel: string;
    nodeAutoColorBy: string;
    linkDirectionalParticles: string;
    linkDirectionalParticleSpeed: (link: GraphLink) => number;
  }

  export class ForceGraph3D extends React.Component<ForceGraph3DProps> {}
}
