import { useState, useEffect } from 'react';
import { 
  Terminal, 
  Server, 
  RefreshCw, 
  Globe, 
  Clock,
  ShieldCheck,
  Cpu,
  HardDrive,
  Database,
  Radio,
  FileCode,
  Layers,
  ChevronRight
} from 'lucide-react';

interface DeviceItem {
  id: string;
  name: string;
  ip: string;
  url: string;
  statusCode: number;
  statusText: string;
  isOnline: boolean;
  latencyMs: number | null;
}

interface CollectorQuery {
  id: string;
  name: string;
  category: 'web_grabber' | 'snmp' | 'script' | 'http';
  target: string;
  interval: number;
  status: string;
  lastRun: string;
  value: string;
}

const FALLBACK_BANNER = ` ┌──────────────────────────────────────────────────────────────────────┐
    │                 • MobaXterm Personal Edition v26.4 •                 │
    │               (SSH client, X server and network tools)               │
    │                                                                      │
    │ ⮞ SSH session to root@10.20.110.7                                    │
    │   • Direct SSH      :  ✓                                             │
    │   • SSH compression :  ✗                                             │
    │   • SSH-browser     :  ✓                                             │
    │   • X11-forwarding  :  ✓  (remote display is forwarded through SSH)  │
    │                                                                      │
    │ ⮞ For more info, ctrl+click on help or visit our website.            │
    └──────────────────────────────────────────────────────────────────────┘

Welcome to Ubuntu 26.04 LTS (GNU/Linux 7.0.0-34-generic x86_64)

 * Documentation:  https://docs.ubuntu.com
 * Management:     https://landscape.canonical.com
 * Support:        https://ubuntu.com/pro

 System information as of Wed Oct 21 01:26:05 AM CEST 2026

  System load:  0.18               Processes:              144
  Usage of /:   44.3% of 23.45GB   Users logged in:        0
  Memory usage: 3%                 IPv4 address for ens18: 10.20.110.7
  Swap usage:   0%

 * Canonical Workshop gives developers fast, composable, reproducible, and
   secure developer environments that are perfect for agentic workflows.

   https://ubuntu.com/workshop

Expanded Security Maintenance for Applications is not enabled.

79 updates can be applied immediately.
11 of these updates are standard security updates.
To see these additional updates run: apt list --upgradable

Enable ESM Apps to receive additional future security updates.
See https://ubuntu.com/esm or run: sudo pro status


*** System restart required ***
Last login: Tue Oct 20 17:09:41 2026 from 10.20.110.22
root@absolutsrf:~#`;

export default function App() {
  const [bannerText, setBannerText] = useState<string>(FALLBACK_BANNER);
  const [devices, setDevices] = useState<DeviceItem[]>([
    { id: 'gw-router', name: 'Main Router / WebUI', ip: '192.168.1.1', url: 'http://192.168.1.1', statusCode: 200, statusText: '200 OK (Scraped)', isOnline: true, latencyMs: 1 },
    { id: 'srf-main', name: 'Main Server (Ubuntu 26.04)', ip: '10.20.110.7', url: 'http://10.20.110.7', statusCode: 200, statusText: '200 OK', isOnline: true, latencyMs: 14 },
    { id: 'client-gw', name: 'Workstation Gateway', ip: '10.20.110.22', url: 'http://10.20.110.22', statusCode: 200, statusText: '200 OK', isOnline: true, latencyMs: 6 },
    { id: 'mgmt-console', name: 'Management Portal', ip: '10.20.110.1', url: 'http://10.20.110.1', statusCode: 401, statusText: '401 Unauthorized', isOnline: true, latencyMs: 22 },
    { id: 'dns-primary', name: 'Primary DNS Node', ip: '1.1.1.1', url: 'https://1.1.1.1', statusCode: 200, statusText: '200 OK', isOnline: true, latencyMs: 18 },
  ]);
  const [queries, setQueries] = useState<CollectorQuery[]>([
    { id: 'q1', category: 'web_grabber', name: 'Router WAN Status', target: 'http://192.168.1.1', interval: 10, status: 'OK', lastRun: '01:31:02', value: 'Connected (PPPoE/Fiber)' },
    { id: 'q2', category: 'web_grabber', name: 'Router Public IP', target: 'http://192.168.1.1', interval: 15, status: 'OK', lastRun: '01:31:10', value: '84.115.12.98' },
    { id: 'q3', category: 'web_grabber', name: 'DSL/Fiber Sync Rate', target: 'http://192.168.1.1', interval: 30, status: 'OK', lastRun: '01:30:45', value: '1000/500 Mbps' },
    { id: 'q4', category: 'web_grabber', name: 'DHCP Client Leases', target: 'http://192.168.1.1', interval: 20, status: 'OK', lastRun: '01:31:12', value: '28 active leases' },
    { id: 'q7', category: 'snmp', name: 'System Uptime (Router)', target: '192.168.1.1', interval: 15, status: 'OK', lastRun: '01:31:05', value: '244d 16h 32m' },
    { id: 'q9', category: 'snmp', name: 'eth0 Inbound Traffic', target: '192.168.1.1', interval: 10, status: 'OK', lastRun: '01:31:15', value: '142.8 Mbps' },
    { id: 'q17', category: 'script', name: 'Ping Latency (192.168.1.1)', target: '192.168.1.1', interval: 5, status: 'OK', lastRun: '01:31:18', value: '0.85 ms' },
    { id: 'q25', category: 'http', name: 'Router WebUI Health', target: 'http://192.168.1.1', interval: 10, status: 'OK', lastRun: '01:31:10', value: 'HTTP 200 OK' }
  ]);
  const [selectedIp, setSelectedIp] = useState('10.20.110.7');
  const [activeTab, setActiveTab] = useState<'both' | 'terminal' | 'devices' | 'collector'>('both');
  const [selectedCategory, setSelectedCategory] = useState<string>('all');
  const [loading, setLoading] = useState(false);
  const [routerGrabResult, setRouterGrabResult] = useState<any>(null);

  const fetchAllData = async () => {
    setLoading(true);
    try {
      // 1. Fetch Terminal Banner
      const bannerRes = await fetch('/api/banner', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ ip: selectedIp })
      });
      if (bannerRes.ok) {
        const data = await bannerRes.json();
        if (data.banner) setBannerText(data.banner);
      }

      // 2. Fetch Devices Status
      const devRes = await fetch('/api/devices', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ devices })
      });
      if (devRes.ok) {
        const data = await devRes.json();
        if (data.devices) setDevices(data.devices);
      }

      // 3. Fetch 30 Queries Collector Status
      const qRes = await fetch('/api/collector/queries', { method: 'POST' });
      if (qRes.ok) {
        const data = await qRes.json();
        if (data.queries && data.queries.length > 0) {
          setQueries(data.queries);
        }
      }
    } catch {
      // Offline fallback
    } finally {
      setLoading(false);
    }
  };

  const triggerRouterGrab = async () => {
    setLoading(true);
    try {
      const res = await fetch('/api/collector/grab_router', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ url: 'http://192.168.1.1' })
      });
      if (res.ok) {
        const d = await res.json();
        setRouterGrabResult(d.data);
      }
    } catch {
      setRouterGrabResult({ error: 'Failed to contact backend scraper.' });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAllData();
    const interval = setInterval(fetchAllData, 10000);
    return () => clearInterval(interval);
  }, [selectedIp]);

  const filteredQueries = selectedCategory === 'all' 
    ? queries 
    : queries.filter(q => q.category === selectedCategory);

  return (
    <div className="min-h-screen bg-[#090d16] text-[#c9d1d9] flex flex-col font-mono selection:bg-[#238636] selection:text-white">
      {/* Top Header */}
      <header className="border-b border-[#21262d] bg-[#161b22] px-6 py-3 flex flex-wrap items-center justify-between gap-4 sticky top-0 z-30 shadow-md">
        <div className="flex items-center gap-3">
          <div className="p-2 rounded-lg bg-[#238636]/20 border border-[#238636]/40 text-[#3fb950]">
            <Terminal className="w-5 h-5" />
          </div>
          <div>
            <h1 className="text-sm font-semibold tracking-wide text-white flex items-center gap-2">
              Multi-System Collector & MobaXterm Console
              <span className="text-[10px] uppercase font-bold px-2 py-0.5 rounded-full bg-[#1f6feb]/20 text-[#58a6ff] border border-[#1f6feb]/40">
                SNMP • Web Grabber • Scripts
              </span>
            </h1>
            <p className="text-xs text-[#8b949e]">Target: {selectedIp} | Gateway: 192.168.1.1</p>
          </div>
        </div>

        {/* View Switcher & Action */}
        <div className="flex items-center gap-3">
          <div className="flex bg-[#0d1117] p-1 rounded-md border border-[#30363d] text-xs">
            <button
              onClick={() => setActiveTab('both')}
              className={`px-3 py-1 rounded transition ${activeTab === 'both' ? 'bg-[#238636] text-white font-medium' : 'text-[#8b949e] hover:text-white'}`}
            >
              Unified View
            </button>
            <button
              onClick={() => setActiveTab('collector')}
              className={`px-3 py-1 rounded transition ${activeTab === 'collector' ? 'bg-[#238636] text-white font-medium' : 'text-[#8b949e] hover:text-white'}`}
            >
              30 Queries ({queries.length})
            </button>
            <button
              onClick={() => setActiveTab('terminal')}
              className={`px-3 py-1 rounded transition ${activeTab === 'terminal' ? 'bg-[#238636] text-white font-medium' : 'text-[#8b949e] hover:text-white'}`}
            >
              SSH Banner
            </button>
            <button
              onClick={() => setActiveTab('devices')}
              className={`px-3 py-1 rounded transition ${activeTab === 'devices' ? 'bg-[#238636] text-white font-medium' : 'text-[#8b949e] hover:text-white'}`}
            >
              Devices HTTP
            </button>
          </div>

          <button
            onClick={fetchAllData}
            disabled={loading}
            className="flex items-center gap-1.5 px-3 py-1.5 text-xs bg-[#21262d] hover:bg-[#30363d] text-[#c9d1d9] border border-[#30363d] rounded-md transition disabled:opacity-50"
          >
            <RefreshCw className={`w-3.5 h-3.5 ${loading ? 'animate-spin text-[#58a6ff]' : ''}`} />
            <span>{loading ? 'Polling...' : 'Refresh'}</span>
          </button>
        </div>
      </header>

      {/* Main Grid */}
      <main className="flex-1 p-4 md:p-6 grid grid-cols-1 lg:grid-cols-12 gap-6 max-w-[1700px] w-full mx-auto">
        
        {/* Terminal Screen Container */}
        {(activeTab === 'both' || activeTab === 'terminal') && (
          <div className={`${activeTab === 'both' ? 'lg:col-span-6' : 'lg:col-span-12'} flex flex-col`}>
            <div className="rounded-lg overflow-hidden border border-[#30363d] bg-[#0c1017] shadow-2xl flex flex-col flex-1">
              <div className="bg-[#161b22] px-4 py-2 border-b border-[#30363d] flex items-center justify-between select-none">
                <div className="flex items-center gap-2">
                  <div className="w-3 h-3 rounded-full bg-[#f85149] inline-block"></div>
                  <div className="w-3 h-3 rounded-full bg-[#d29922] inline-block"></div>
                  <div className="w-3 h-3 rounded-full bg-[#3fb950] inline-block"></div>
                  <span className="ml-2 text-xs text-[#8b949e] font-mono">root@absolutsrf: ~ (MobaXterm v26.4 SSH)</span>
                </div>
                <span className="text-[#3fb950] text-xs flex items-center gap-1">
                  <span className="w-2 h-2 rounded-full bg-[#3fb950] animate-pulse"></span>
                  CONNECTED
                </span>
              </div>

              {/* Terminal Body */}
              <div className="p-4 md:p-6 overflow-x-auto text-[13px] leading-relaxed text-[#58a6ff] bg-[#05080e] min-h-[460px] flex-1 flex flex-col justify-between">
                <pre className="whitespace-pre font-mono text-[#c9d1d9] select-text">
                  {bannerText}
                </pre>
                
                <div className="mt-4 flex items-center text-[#3fb950]">
                  <span className="text-[#58a6ff]">root@absolutsrf</span>
                  <span className="text-white">:</span>
                  <span className="text-[#d29922]">~</span>
                  <span className="text-white">#&nbsp;</span>
                  <span className="inline-block w-2.5 h-4 bg-[#3fb950] animate-pulse"></span>
                </div>
              </div>

              {/* Terminal Bottom stats */}
              <div className="bg-[#161b22] px-4 py-2 border-t border-[#30363d] text-[11px] text-[#8b949e] flex flex-wrap items-center justify-between gap-2">
                <div className="flex items-center gap-4">
                  <span className="flex items-center gap-1"><Cpu className="w-3.5 h-3.5 text-[#58a6ff]" /> Load: 0.18</span>
                  <span className="flex items-center gap-1"><HardDrive className="w-3.5 h-3.5 text-[#e3b341]" /> Root: 44.3%</span>
                </div>
                <span className="text-[#58a6ff] font-medium flex items-center gap-1">
                  <ShieldCheck className="w-3.5 h-3.5" /> 10.20.110.7 session active
                </span>
              </div>
            </div>
          </div>
        )}

        {/* Right / Secondary Section: Collector & Devices */}
        <div className={`${activeTab === 'both' ? 'lg:col-span-6' : 'lg:col-span-12'} flex flex-col gap-5`}>
          
          {/* Quick Scraper Banner for 192.168.1.1 */}
          <div className="bg-[#161b22] border border-[#30363d] rounded-lg p-4 shadow-sm">
            <div className="flex flex-wrap items-center justify-between gap-3">
              <div className="flex items-center gap-3">
                <div className="p-2 rounded bg-[#1f6feb]/20 text-[#58a6ff]">
                  <Globe className="w-5 h-5" />
                </div>
                <div>
                  <h3 className="text-sm font-semibold text-white">Target Gateway Scraper: 192.168.1.1</h3>
                  <p className="text-xs text-[#8b949e]">Bypasses clunky router web interface and extracts raw values</p>
                </div>
              </div>
              <button
                onClick={triggerRouterGrab}
                className="px-3 py-1.5 text-xs bg-[#238636] hover:bg-[#2ea043] text-white rounded font-medium transition flex items-center gap-1.5 shadow"
              >
                <span>Scrape Router HTML Now</span>
                <ChevronRight className="w-3.5 h-3.5" />
              </button>
            </div>

            {routerGrabResult && (
              <div className="mt-3 p-3 bg-[#0d1117] border border-[#30363d] rounded text-xs">
                <div className="flex justify-between items-center text-[#8b949e] border-b border-[#21262d] pb-1.5 mb-2">
                  <span className="font-semibold text-white">Parser Result: {routerGrabResult.title || 'Router WebUI'}</span>
                  <span className="text-[#3fb950] font-mono">Status: {routerGrabResult.statusCode || 200}</span>
                </div>
                <p className="text-[#8b949e] font-mono mb-2">{routerGrabResult.cleanTextSnippet}</p>
                {routerGrabResult.extractedIps && (
                  <div className="flex items-center gap-1 text-[11px]">
                    <span className="text-[#58a6ff]">Extracted Nodes:</span>
                    {routerGrabResult.extractedIps.map((ip: string, idx: number) => (
                      <span key={idx} className="bg-[#21262d] px-1.5 py-0.5 rounded text-[#c9d1d9]">{ip}</span>
                    ))}
                  </div>
                )}
              </div>
            )}
          </div>

          {/* 30 Multi-Source Queries Overview */}
          {(activeTab === 'both' || activeTab === 'collector') && (
            <div className="bg-[#161b22] border border-[#30363d] rounded-lg overflow-hidden flex flex-col flex-1 shadow-md">
              <div className="px-4 py-3 border-b border-[#30363d] flex flex-wrap items-center justify-between gap-2">
                <div className="flex items-center gap-2">
                  <Layers className="w-4 h-4 text-[#58a6ff]" />
                  <h2 className="text-sm font-semibold text-white">Configured Data Queries (~30 Active)</h2>
                </div>

                {/* Filter Pills */}
                <div className="flex gap-1.5 text-xs">
                  {['all', 'web_grabber', 'snmp', 'script', 'http'].map((cat) => (
                    <button
                      key={cat}
                      onClick={() => setSelectedCategory(cat)}
                      className={`px-2 py-0.5 rounded transition capitalize ${selectedCategory === cat ? 'bg-[#388bfd]/20 text-[#58a6ff] border border-[#388bfd]/50' : 'text-[#8b949e] hover:text-white'}`}
                    >
                      {cat.replace('_', ' ')}
                    </button>
                  ))}
                </div>
              </div>

              {/* Query Matrix Items */}
              <div className="divide-y divide-[#21262d] overflow-y-auto max-h-[350px]">
                {filteredQueries.map((q) => {
                  let icon = <Database className="w-4 h-4 text-[#58a6ff]" />;
                  if (q.category === 'web_grabber') icon = <Globe className="w-4 h-4 text-[#3fb950]" />;
                  if (q.category === 'snmp') icon = <Radio className="w-4 h-4 text-[#e3b341]" />;
                  if (q.category === 'script') icon = <FileCode className="w-4 h-4 text-[#f778ba]" />;

                  return (
                    <div key={q.id} className="p-3 hover:bg-[#1a202c] transition flex items-center justify-between gap-3">
                      <div className="flex items-center gap-3">
                        <div className="p-1.5 rounded bg-[#0d1117] border border-[#30363d]">
                          {icon}
                        </div>
                        <div>
                          <div className="flex items-center gap-2">
                            <span className="text-xs font-semibold text-white">{q.name}</span>
                            <span className="text-[10px] uppercase font-mono px-1.5 rounded bg-[#21262d] text-[#8b949e]">
                              {q.category}
                            </span>
                          </div>
                          <span className="text-[11px] text-[#8b949e] font-mono">{q.target}</span>
                        </div>
                      </div>

                      <div className="text-right">
                        <span className="text-xs font-mono font-bold text-[#3fb950] block">{q.value}</span>
                        <span className="text-[10px] text-[#8b949e] flex items-center justify-end gap-1">
                          <Clock className="w-3 h-3" /> every {q.interval}s
                        </span>
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>
          )}

          {/* Network Devices HTTP Status Overview */}
          {(activeTab === 'both' || activeTab === 'devices') && (
            <div className="bg-[#161b22] border border-[#30363d] rounded-lg overflow-hidden flex flex-col shadow-md">
              <div className="px-4 py-3 border-b border-[#30363d] flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <Server className="w-4 h-4 text-[#58a6ff]" />
                  <h2 className="text-sm font-semibold text-white">Device HTTP Availability Probe</h2>
                </div>
                <span className="text-xs text-[#3fb950] font-mono">
                  {devices.filter(d => d.isOnline).length} / {devices.length} Online
                </span>
              </div>

              <div className="divide-y divide-[#21262d] overflow-y-auto max-h-[220px]">
                {devices.map((device) => {
                  const isSuccess = device.statusCode >= 200 && device.statusCode < 300;
                  return (
                    <div 
                      key={device.id} 
                      className={`p-3 transition hover:bg-[#1a202c] cursor-pointer flex items-center justify-between gap-2 ${selectedIp === device.ip ? 'bg-[#1c222c] border-l-4 border-l-[#58a6ff]' : ''}`}
                      onClick={() => setSelectedIp(device.ip)}
                    >
                      <div>
                        <div className="flex items-center gap-2">
                          <span className="text-xs font-medium text-white">{device.name}</span>
                          <span className="text-[11px] text-[#8b949e] font-mono">({device.ip})</span>
                        </div>
                        <span className="text-[11px] text-[#58a6ff] truncate block max-w-[280px]">{device.url}</span>
                      </div>

                      <div className="flex items-center gap-2">
                        {device.latencyMs !== null && (
                          <span className="text-[11px] font-mono text-[#3fb950]">{device.latencyMs}ms</span>
                        )}
                        <span className={`text-[11px] px-2 py-0.5 rounded border font-mono font-bold ${isSuccess ? 'bg-[#238636]/20 text-[#3fb950] border-[#238636]/40' : 'bg-[#f85149]/20 text-[#f85149] border-[#f85149]/40'}`}>
                          HTTP {device.statusCode}
                        </span>
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>
          )}

        </div>
      </main>
    </div>
  );
}
