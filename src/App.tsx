import { useState, useEffect } from 'react';
import { 
  Server, 
  RefreshCw, 
  Clock, 
  ShieldCheck, 
  Cpu, 
  HardDrive, 
  Radio, 
  Layers, 
  ArrowDownCircle, 
  ArrowUpCircle,
  Activity,
  CheckCircle2,
  XCircle,
  Search,
  Network,
  Smartphone,
  Router
} from 'lucide-react';

interface DeviceCategory {
  Id: number;
  Key: string;
  Name: string;
  ShortName: string;
  Description: string;
}

interface FiveGDevice {
  Id: number;
  Name: string;
  IpAddress: string;
  Port: number;
  CategoryId: number;
  CategoryName: string;
  CategoryKey: string;
  LastReachable: number;
  LastPolledAt: string;
  CollectionMethod: number;
}

interface ThroughputItem {
  Timestamp: string;
  DlMbps: number;
  UlMbps: number;
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

 System information as of Wed Oct 21 02:30:10 AM CEST 2026

  System load:  0.18               Processes:              144
  Usage of /:   44.3% of 23.45GB   Users logged in:        0
  Memory usage: 3%                 IPv4 address for ens18: 10.20.110.7
  Swap usage:   0%

root@absolutsrf:~#`;

export default function App() {
  const [categories, setCategories] = useState<DeviceCategory[]>([
    { Id: 1, Key: "mobile", Name: "5G Core", ShortName: "Core", Description: "5G mobile core network functions." },
    { Id: 2, Key: "ims", Name: "IMS Core", ShortName: "IMS", Description: "IMS core network functions." },
    { Id: 3, Key: "bbu", Name: "Base Stations", ShortName: "BBU", Description: "5G base stations / gNodeB." },
    { Id: 4, Key: "au", Name: "Access Units", ShortName: "AU", Description: "Aggregation units." },
    { Id: 5, Key: "eu", Name: "Hub & Power", ShortName: "EU", Description: "Remote hub & power units." },
    { Id: 6, Key: "ru", Name: "PICO Sender", ShortName: "RU", Description: "Radio units." },
    { Id: 7, Key: "cell", Name: "Cells", ShortName: "Cell", Description: "NR/LTE cells." },
    { Id: 8, Key: "ue", Name: "User Equipment", ShortName: "UE", Description: "Connected handsets." },
    { Id: 9, Key: "switch", Name: "Switches", ShortName: "SW", Description: "Ethernet switches." }
  ]);

  const [devices, setDevices] = useState<FiveGDevice[]>([
    { Id: 1, Name: "H3C vSCN5100 5G Core", IpAddress: "10.20.110.15", Port: 22, CategoryId: 1, CategoryName: "5G Core", CategoryKey: "mobile", LastReachable: 1, LastPolledAt: "Just now", CollectionMethod: 1 },
    { Id: 2, Name: "H3C 5gVIMS Core", IpAddress: "10.20.110.16", Port: 161, CategoryId: 2, CategoryName: "IMS Core", CategoryKey: "ims", LastReachable: 1, LastPolledAt: "Just now", CollectionMethod: 0 },
    { Id: 3, Name: "BTI nCELL-T5100 (Site Alpha)", IpAddress: "10.20.110.30", Port: 20000, CategoryId: 3, CategoryName: "Base Stations", CategoryKey: "bbu", LastReachable: 1, LastPolledAt: "Just now", CollectionMethod: 3 },
    { Id: 4, Name: "Huawei S5735 Switch", IpAddress: "10.20.110.1", Port: 161, CategoryId: 9, CategoryName: "Switches", CategoryKey: "switch", LastReachable: 1, LastPolledAt: "Just now", CollectionMethod: 0 },
    { Id: 5, Name: "BTI AU300 Aggregation", IpAddress: "10.20.110.30", Port: 0, CategoryId: 4, CategoryName: "Access Units", CategoryKey: "au", LastReachable: 1, LastPolledAt: "Just now", CollectionMethod: 2 },
    { Id: 6, Name: "BTI RU8250 PICO Sender", IpAddress: "10.20.110.30", Port: 0, CategoryId: 6, CategoryName: "PICO Sender", CategoryKey: "ru", LastReachable: 1, LastPolledAt: "Just now", CollectionMethod: 2 },
    { Id: 7, Name: "NR Cell 1 (3.5GHz n78)", IpAddress: "10.20.110.30", Port: 0, CategoryId: 7, CategoryName: "Cells", CategoryKey: "cell", LastReachable: 1, LastPolledAt: "Just now", CollectionMethod: 2 },
    { Id: 8, Name: "Connected Handset (IMSI ...489)", IpAddress: "10.45.0.2", Port: 0, CategoryId: 8, CategoryName: "User Equipment", CategoryKey: "ue", LastReachable: 1, LastPolledAt: "Just now", CollectionMethod: 1 }
  ]);

  const [throughput, setThroughput] = useState<ThroughputItem[]>([
    { Timestamp: "02:34:00", DlMbps: 684.2, UlMbps: 112.5 },
    { Timestamp: "02:33:30", DlMbps: 712.0, UlMbps: 108.4 }
  ]);

  const [selectedCategory, setSelectedCategory] = useState<string>("all");
  const [searchFilter, setSearchFilter] = useState<string>("");
  const [activeTab, setActiveTab] = useState<'both' | 'devices' | 'terminal'>('both');
  const [loading, setLoading] = useState<boolean>(false);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await fetch('/api/fiveg/data');
      if (res.ok) {
        const data = await res.json();
        if (data.categories && data.categories.length > 0) setCategories(data.categories);
        if (data.devices && data.devices.length > 0) setDevices(data.devices);
        if (data.throughput && data.throughput.length > 0) setThroughput(data.throughput);
      }
    } catch {
      // offline fallback
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
    const interval = setInterval(fetchData, 8000);
    return () => clearInterval(interval);
  }, []);

  const filteredDevices = devices.filter(d => {
    const matchesCat = selectedCategory === "all" || d.CategoryKey === selectedCategory;
    const matchesSearch = !searchFilter || 
      d.Name.toLowerCase().includes(searchFilter.toLowerCase()) || 
      d.IpAddress.toLowerCase().includes(searchFilter.toLowerCase());
    return matchesCat && matchesSearch;
  });

  const latestThroughput = throughput[0] || { DlMbps: 684.2, UlMbps: 112.5 };
  const onlineCount = devices.filter(d => d.LastReachable !== 0).length;

  return (
    <div className="min-h-screen bg-[#090d16] text-[#c9d1d9] flex flex-col font-mono selection:bg-[#238636] selection:text-white">
      {/* Top Navbar */}
      <header className="border-b border-[#21262d] bg-[#161b22] px-6 py-3 flex flex-wrap items-center justify-between gap-4 sticky top-0 z-30 shadow-md">
        <div className="flex items-center gap-3">
          <div className="p-2 rounded-lg bg-[#238636]/20 border border-[#238636]/40 text-[#3fb950]">
            <Radio className="w-5 h-5 animate-pulse" />
          </div>
          <div>
            <h1 className="text-sm font-semibold tracking-wide text-white flex items-center gap-2">
              5G Network Operations & MobaXterm Live Console
              <span className="text-[10px] uppercase font-bold px-2 py-0.5 rounded-full bg-[#1f6feb]/20 text-[#58a6ff] border border-[#1f6feb]/40">
                5G Core • BBU • AU • RU • UE
              </span>
            </h1>
            <p className="text-xs text-[#8b949e]">SQLite /var/lib/absolutdashboard-test/app.db | Host: 10.20.110.7</p>
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
              onClick={() => setActiveTab('devices')}
              className={`px-3 py-1 rounded transition ${activeTab === 'devices' ? 'bg-[#238636] text-white font-medium' : 'text-[#8b949e] hover:text-white'}`}
            >
              5G Devices ({devices.length})
            </button>
            <button
              onClick={() => setActiveTab('terminal')}
              className={`px-3 py-1 rounded transition ${activeTab === 'terminal' ? 'bg-[#238636] text-white font-medium' : 'text-[#8b949e] hover:text-white'}`}
            >
              MobaXterm Console
            </button>
          </div>

          <button
            onClick={fetchData}
            disabled={loading}
            className="flex items-center gap-1.5 px-3 py-1.5 text-xs bg-[#21262d] hover:bg-[#30363d] text-[#c9d1d9] border border-[#30363d] rounded-md transition disabled:opacity-50"
          >
            <RefreshCw className={`w-3.5 h-3.5 ${loading ? 'animate-spin text-[#58a6ff]' : ''}`} />
            <span>{loading ? 'Refreshing...' : 'Refresh'}</span>
          </button>
        </div>
      </header>

      {/* Main Content Area */}
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
                  {FALLBACK_BANNER}
                </pre>
                
                <div className="mt-4 flex items-center text-[#3fb950]">
                  <span className="text-[#58a6ff]">root@absolutsrf</span>
                  <span className="text-white">:</span>
                  <span className="text-[#d29922]">~</span>
                  <span className="text-white">#&nbsp;</span>
                  <span className="inline-block w-2.5 h-4 bg-[#3fb950] animate-pulse"></span>
                </div>
              </div>

              {/* Terminal Quick stats */}
              <div className="bg-[#161b22] px-4 py-2 border-t border-[#30363d] text-[11px] text-[#8b949e] flex flex-wrap items-center justify-between gap-2">
                <div className="flex items-center gap-4">
                  <span className="flex items-center gap-1"><Cpu className="w-3.5 h-3.5 text-[#58a6ff]" /> Load: 0.18</span>
                  <span className="flex items-center gap-1"><HardDrive className="w-3.5 h-3.5 text-[#e3b341]" /> Root: 44.3%</span>
                  <span className="flex items-center gap-1"><Activity className="w-3.5 h-3.5 text-[#3fb950]" /> RAM: 3%</span>
                </div>
                <span className="text-[#58a6ff] font-medium flex items-center gap-1">
                  <ShieldCheck className="w-3.5 h-3.5" /> 10.20.110.7 session active
                </span>
              </div>
            </div>
          </div>
        )}

        {/* 5G Network Dashboard Container */}
        {(activeTab === 'both' || activeTab === 'devices') && (
          <div className={`${activeTab === 'both' ? 'lg:col-span-6' : 'lg:col-span-12'} flex flex-col gap-4`}>
            
            {/* 5G Live Throughput & Metrics Cards */}
            <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
              <div className="bg-[#161b22] border border-[#30363d] p-3 rounded-lg shadow-sm">
                <span className="text-xs text-[#8b949e] flex items-center gap-1.5">
                  <ArrowDownCircle className="w-3.5 h-3.5 text-[#3fb950]" /> 5G Downlink
                </span>
                <p className="text-lg font-bold text-white mt-1">{latestThroughput.DlMbps} <span className="text-xs text-[#8b949e]">Mbps</span></p>
              </div>

              <div className="bg-[#161b22] border border-[#30363d] p-3 rounded-lg shadow-sm">
                <span className="text-xs text-[#8b949e] flex items-center gap-1.5">
                  <ArrowUpCircle className="w-3.5 h-3.5 text-[#58a6ff]" /> 5G Uplink
                </span>
                <p className="text-lg font-bold text-white mt-1">{latestThroughput.UlMbps} <span className="text-xs text-[#8b949e]">Mbps</span></p>
              </div>

              <div className="bg-[#161b22] border border-[#30363d] p-3 rounded-lg shadow-sm">
                <span className="text-xs text-[#8b949e] flex items-center gap-1.5">
                  <Network className="w-3.5 h-3.5 text-[#d29922]" /> 5G Nodes
                </span>
                <p className="text-lg font-bold text-white mt-1">{devices.length}</p>
              </div>

              <div className="bg-[#161b22] border border-[#30363d] p-3 rounded-lg shadow-sm">
                <span className="text-xs text-[#8b949e] flex items-center gap-1.5">
                  <CheckCircle2 className="w-3.5 h-3.5 text-[#3fb950]" /> Operational
                </span>
                <p className="text-lg font-bold text-[#3fb950] mt-1">{onlineCount} / {devices.length}</p>
              </div>
            </div>

            {/* 5G Topology & Devices List */}
            <div className="bg-[#161b22] border border-[#30363d] rounded-lg overflow-hidden flex flex-col flex-1 shadow-md">
              
              {/* Header with Search and Category Filter */}
              <div className="p-3 border-b border-[#30363d] flex flex-col gap-2.5">
                <div className="flex items-center justify-between gap-2">
                  <div className="flex items-center gap-2">
                    <Layers className="w-4 h-4 text-[#58a6ff]" />
                    <h2 className="text-sm font-semibold text-white">5G Network Elements & Devices</h2>
                  </div>

                  {/* Search box */}
                  <div className="relative">
                    <Search className="w-3.5 h-3.5 absolute left-2.5 top-2 text-[#8b949e]" />
                    <input 
                      type="text"
                      placeholder="Search device / IP..."
                      value={searchFilter}
                      onChange={(e) => setSearchFilter(e.target.value)}
                      className="bg-[#0d1117] border border-[#30363d] rounded text-xs pl-8 pr-3 py-1 text-white placeholder-[#8b949e] focus:outline-none focus:border-[#58a6ff]"
                    />
                  </div>
                </div>

                {/* Category Pills */}
                <div className="flex flex-wrap gap-1.5 text-[11px]">
                  <button
                    onClick={() => setSelectedCategory("all")}
                    className={`px-2 py-0.5 rounded transition ${selectedCategory === "all" ? 'bg-[#388bfd] text-white font-medium' : 'bg-[#21262d] text-[#8b949e] hover:text-white'}`}
                  >
                    All ({devices.length})
                  </button>
                  {categories.map((c) => {
                    const count = devices.filter(d => d.CategoryKey === c.Key).length;
                    return (
                      <button
                        key={c.Id}
                        onClick={() => setSelectedCategory(c.Key)}
                        className={`px-2 py-0.5 rounded transition ${selectedCategory === c.Key ? 'bg-[#388bfd] text-white font-medium' : 'bg-[#21262d] text-[#8b949e] hover:text-white'}`}
                      >
                        {c.ShortName} ({count})
                      </button>
                    );
                  })}
                </div>
              </div>

              {/* Devices Scroll Area */}
              <div className="divide-y divide-[#21262d] overflow-y-auto max-h-[500px]">
                {filteredDevices.map((dev) => {
                  let icon = <Server className="w-4 h-4 text-[#58a6ff]" />;
                  if (dev.CategoryKey === 'bbu') icon = <Radio className="w-4 h-4 text-[#3fb950]" />;
                  if (dev.CategoryKey === 'ue') icon = <Smartphone className="w-4 h-4 text-[#e3b341]" />;
                  if (dev.CategoryKey === 'switch') icon = <Router className="w-4 h-4 text-[#f778ba]" />;

                  const isOnline = dev.LastReachable !== 0;

                  return (
                    <div key={dev.Id} className="p-3.5 hover:bg-[#1a202c] transition flex items-center justify-between gap-3">
                      <div className="flex items-center gap-3">
                        <div className="p-2 rounded bg-[#0d1117] border border-[#30363d]">
                          {icon}
                        </div>
                        <div>
                          <div className="flex items-center gap-2">
                            <span className="text-xs font-semibold text-white">{dev.Name}</span>
                            <span className="text-[10px] px-1.5 py-0.2 rounded bg-[#21262d] text-[#8b949e] font-mono">
                              {dev.CategoryName}
                            </span>
                          </div>
                          <span className="text-[11px] text-[#8b949e] font-mono">
                            {dev.IpAddress} {dev.Port > 0 ? `:${dev.Port}` : ''}
                          </span>
                        </div>
                      </div>

                      <div className="text-right flex items-center gap-3">
                        <div className="hidden sm:block">
                          <span className="text-[10px] text-[#8b949e] block font-mono">
                            {dev.LastPolledAt || 'Polled'}
                          </span>
                          <span className="text-[10px] text-[#58a6ff] uppercase">
                            {dev.CollectionMethod === 0 ? 'SNMP' : dev.CollectionMethod === 1 ? 'SSH' : dev.CollectionMethod === 3 ? 'BTI Protocol' : 'Via Parent'}
                          </span>
                        </div>

                        <div className={`px-2 py-0.5 rounded text-[11px] font-bold border flex items-center gap-1 ${isOnline ? 'bg-[#238636]/20 text-[#3fb950] border-[#238636]/40' : 'bg-[#f85149]/20 text-[#f85149] border-[#f85149]/40'}`}>
                          {isOnline ? <CheckCircle2 className="w-3 h-3 text-[#3fb950]" /> : <XCircle className="w-3 h-3 text-[#f85149]" />}
                          <span>{isOnline ? 'ONLINE' : 'DOWN'}</span>
                        </div>
                      </div>
                    </div>
                  );
                })}
              </div>

              {/* Bottom Card Footer */}
              <div className="p-3 bg-[#0d1117] border-t border-[#30363d] text-xs text-[#8b949e] flex items-center justify-between">
                <span className="flex items-center gap-1.5">
                  <ShieldCheck className="w-3.5 h-3.5 text-[#3fb950]" />
                  Synchronized with /var/lib/absolutdashboard-test/app.db
                </span>
                <span className="flex items-center gap-1">
                  <Clock className="w-3 h-3" /> Auto-sync 8s
                </span>
              </div>
            </div>
          </div>
        )}

      </main>
    </div>
  );
}
