const PROXY_CONFIG = [
  {
    context: [
      "/api",
    ],
    target: "https://localhost:7023",
    pathRewrite: { '^/api': '' },
    secure: false
  },
  {
    context: () => true,
    target: "https://localhost:7023",
    secure: false,
    ws: true,
  },
  {
    context: [
      "/hubs",
    ],
    target: "https://localhost:7023",
    secure: false,
    ws: true,
  }
]

module.exports = PROXY_CONFIG;
